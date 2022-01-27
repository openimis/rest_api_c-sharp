using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenImis.DB.SqlServer;
using OpenImis.ePayment.Controllers;
using OpenImis.ePayment.Data;
using OpenImis.ePayment.Escape.Payment.Models;
using OpenImis.ePayment.Logic;
using OpenImis.ePayment.Models.Payment.Response;
using OpenImis.ModulesV3.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ModulesV3.Helpers
{

    class SelfRenewalHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        
        public SelfRenewalHelper(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<DataMessage> CreateSelfRenewal(SelfRenewal renewal)
        {
            // Log the request for future reference
            GenerateRquestFile(renewal);

            var context = new ImisDB();
            var dataMessage = Validate(renewal);

            // Send SMS for the previously created request
            if (dataMessage.Code == (int)Errors.Renewal.RenewalAlreadyRequested)
            {
                var payment = new ImisPayment(_configuration, _hostingEnvironment);
                
                var paymentId = GetPaymentId(renewal);
                if (paymentId > 0)
                {
                    payment.GetPaymentInfo(Convert.ToInt32(paymentId));

                    // Replace the phone number by the current recipient 
                    payment.PhoneNumber = renewal.Msisdn;
                    SendSMS(payment);
                }
            }

            if (dataMessage.Code != 0)
                return dataMessage;

            // All checks passed, continue creating a new policy in tblPolicy
            
            var insuree = context.TblInsuree
                                .Where(i => i.Chfid == renewal.InsuranceNumber && i.ValidityTo == null)
                                .Include(i => i.Family).ThenInclude(f => f.TblPolicy).ThenInclude(p => p.Prod).ThenInclude(pr => pr.ConversionProd)
                                .FirstOrDefault();

            var insurees = context.TblInsuree
                            .Where(i => i.FamilyId == insuree.FamilyId && i.ValidityTo == null).ToList();

            var product = context.TblProduct.Where(prod => prod.ProductCode == renewal.ProductCode && prod.ValidityTo == null).FirstOrDefault();

            var dtPolicyPeriod = GetPolicyPeriod(product.ProdId, DateTime.Now.Date);

            var prevPolicy = insuree.Family.TblPolicy.Where(p => p.Prod.ProductCode == renewal.ProductCode && p.ValidityTo == null).FirstOrDefault();

            var conProd = insuree.Family.TblPolicy.Any(p => p.Prod.ConversionProd != null);

            TblPolicy convPolicy = null;
            if (conProd == true)

                convPolicy = insuree.Family.TblPolicy.Where(p => p.Prod.ConversionProd.ProductCode == renewal.ProductCode && p.ValidityTo == null).FirstOrDefault();

            int officerId = 0;
            if (prevPolicy != null)
                officerId = (int)prevPolicy.OfficerId;
            else
                officerId = (int)convPolicy.OfficerId;


            // Prepare policy
            var policy = new TblPolicy
            {
                FamilyId = insuree.FamilyId,
                EnrollDate = DateTime.Now,
                StartDate = (DateTime)dtPolicyPeriod.Rows[0]["StartDate"],
                ExpiryDate = (DateTime?)dtPolicyPeriod.Rows[0]["ExpiryDate"],
                PolicyStatus = 1,
                PolicyValue = product.LumpSum, // for now we are taking lumnpsum but in future we might have to consider other paramters like Grace period, discount preiod etc...
                ProdId = product.ProdId,
                OfficerId = officerId,
                ValidityFrom = DateTime.Now,
                AuditUserId = -1,
                IsOffline = false,
                PolicyStage = "R",
                SelfRenewed = true
            };

            // Prepare InsureePolicy
            var insureePolicy = new List<TblInsureePolicy>();
            foreach (var ins in insurees)
            {
                var insPol = new TblInsureePolicy
                {
                    InsureeId = ins.InsureeId,
                    PolicyId = policy.PolicyId,
                    EnrollmentDate = policy.EnrollDate,
                    StartDate = policy.StartDate,
                    EffectiveDate = policy.EffectiveDate,
                    ExpiryDate = policy.ExpiryDate,
                    ValidityFrom = DateTime.Now,
                    AuditUserId = -1,
                    IsOffline = false
                };

                insureePolicy.Add(insPol);
            }

            policy.TblInsureePolicy = insureePolicy;

            var controlNumberResponse = new GetControlNumberResp();
            // Begin transaction

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.TblPolicy.Add(policy);
                    context.SaveChanges();

                    // Request Control Number from the Pool, if we fail to get the control number delete the newly created entry

                    var officer = context.TblOfficer.Where(o => o.OfficerId == policy.OfficerId).FirstOrDefault();
                    var intent = new IntentOfSinglePay
                    {
                        Msisdn = renewal.Msisdn,
                        request_date = DateTime.Now.Date.ToString(),
                        OfficerCode = officer.Code,
                        InsureeNumber = renewal.InsuranceNumber,
                        ProductCode = renewal.ProductCode,
                        EnrolmentType = ePayment.Models.EnrolmentType.Renewal,
                        language = "en"
                    };

                    controlNumberResponse = await GetControlNumber(intent);

                    if (!String.IsNullOrEmpty(controlNumberResponse.control_number))
                    {
                        dbContextTransaction.Commit();
                        dataMessage.Data = context.TblPolicy.Where(p => p.PolicyId == policy.PolicyId).Select(p => new { p.PolicyId, p.EnrollDate, ControlNumber = controlNumberResponse.control_number }).FirstOrDefault();

                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                        dataMessage.Data = controlNumberResponse.error_message;
                    }
                        

                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

          
            return dataMessage;
        }

        private DataMessage Validate(SelfRenewal renewal)
        {
            var dataMessage = new DataMessage();
            var context = new ImisDB();
            var insuree = new TblInsuree();

            //InsuranceNumberNotFound = 3007,
            insuree = context.TblInsuree
                                .Where(i => i.Chfid == renewal.InsuranceNumber && i.ValidityTo == null)
                                .Include(i => i.Family).ThenInclude(f => f.TblPolicy).ThenInclude(p => p.Prod).ThenInclude(pr => pr.ConversionProd)
                                .FirstOrDefault();
            if (insuree == null)
            {
                dataMessage.Code = (int)Errors.Renewal.InsuranceNumberNotFound;
                dataMessage.MessageValue = "Insuree with given insurance number not found";
                dataMessage.ErrorOccured = true;
                return dataMessage;
            }

            //RenewalAlreadyRequested = 3008,
            if (insuree.Family.TblPolicy.Where(p => p.Prod.ProductCode == renewal.ProductCode && p.PolicyStatus == 1 && p.ValidityTo == null).FirstOrDefault() != null)
            {
                dataMessage.Code = (int)Errors.Renewal.RenewalAlreadyRequested;
                dataMessage.MessageValue = "Renewal already created";
                dataMessage.ErrorOccured = true;
                return dataMessage;
            }

            //NoPreviousPolicyFoundToRenew = 3009,
            var prevPolicy = insuree.Family.TblPolicy.Where(p => p.Prod.ProductCode == renewal.ProductCode  && p.ValidityTo == null).FirstOrDefault();
            var conProd = insuree.Family.TblPolicy.Any(p => p.Prod.ConversionProd != null);

            TblPolicy convPolicy = null;
            if (conProd == true)
                convPolicy = insuree.Family.TblPolicy.Where(p => p.Prod.ConversionProd.ProductCode == renewal.ProductCode  && p.ValidityTo == null).FirstOrDefault();
            
            if (prevPolicy == null && convPolicy == null)
            {
                dataMessage.Code = (int)Errors.Renewal.NoPreviousPolicyFoundToRenew;
                dataMessage.MessageValue = "No previous policy found with given renewal product";
                dataMessage.ErrorOccured = true;
                return dataMessage;
            }

            return dataMessage;
        }

        private DataTable GetPolicyPeriod(int productId, DateTime enrolDate)
        {
            var sSQL = "uspGetPolicyPeriod";
            var dh = new DataHelper(_configuration);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@ProdId", productId),
                new SqlParameter("@EnrolDate", enrolDate),
                new SqlParameter("@PolicyStage", "R")
                };

            return dh.GetDataTable(sSQL, sqlParameters, CommandType.StoredProcedure);

            
        }

        private async Task<GetControlNumberResp> GetControlNumber(IntentOfSinglePay intent)
        {
            var result = await new PaymentController(_configuration, _hostingEnvironment).CHFRequestControlNumberForSimplePolicy(intent);
            var result1 = (OkObjectResult)result;
            var value = (GetControlNumberResp)result1.Value;
            
            return value;
        }

        private void GenerateRquestFile(SelfRenewal renewal)
        {
            var currentDate = DateTime.Now.ToString("yyyy/M/d/");
            var currentDateTime = DateTime.Now.ToString("yyyy-M-dTHH-mm-ss");
            string targetPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "SelfRenewals", currentDate);
            
            //if no Directory with current date - then create folder
            if (!Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }
            //we have target folder for current date - then we can save file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(targetPath, renewal.Msisdn + "_" + currentDateTime + ".txt")))
            {
                outputFile.WriteLine(JsonConvert.SerializeObject(renewal));
            }
        }

        private long GetPaymentId(SelfRenewal renewal)
        {
            var sSQL = @"SELECT TOP 1 P.PaymentID 
                        FROM tblPaymentDetails PD 
                        INNER JOIN tblPayment P ON PD.PaymentID = P.PaymentID
                        WHERE PD.ValidityTo IS NULL
                        AND P.ValidityTo IS NULL
                        AND InsuranceNumber = @InsuranceNumber
                        AND ProductCode = @ProductCode
                        AND P.PaymentStatus = 3
                        ORDER BY P.PaymentID DESC;";

            var dh = new DataHelper(_configuration);
            SqlParameter[] parameters =
            {
                new SqlParameter("@InsuranceNumber", renewal.InsuranceNumber),
                new SqlParameter("@ProductCode", renewal.ProductCode)
            };

            var dt = dh.GetDataTable(sSQL, parameters, CommandType.Text);

            if (dt != null && dt.Rows.Count > 0)
                return (long)dt.Rows[0]["PaymentId"];

            return 0;

        }
        
        private void SendSMS(ImisPayment payment)
        {
            var paymentLogic = new PaymentLogic(_configuration, _hostingEnvironment);
            paymentLogic.ControlNumberAssignedSms(payment);
        }


    }
}
