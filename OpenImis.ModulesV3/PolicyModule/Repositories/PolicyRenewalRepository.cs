using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.Helpers;
using OpenImis.ModulesV3.PolicyModule.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using System;
using System.Diagnostics;
using OpenImis.ePayment.Data;
using OpenImis.ModulesV3.Utils;
using OpenImis.ePayment.Logic;
using System.Threading.Tasks;

namespace OpenImis.ModulesV3.PolicyModule.Repositories
{
    public class PolicyRenewalRepository : IPolicyRenewalRepository
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PolicyRenewalRepository(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // TODO: Receiving RenewalUUID directly from SP
        public List<GetPolicyRenewalModel> Get(string officerCode)
        {
            List<GetPolicyRenewalModel> response = new List<GetPolicyRenewalModel>();

            using (var imisContext = new ImisDB())
            {
                var officerCodeParameter = new SqlParameter("@OfficerCode", SqlDbType.NVarChar, 8);
                officerCodeParameter.Value = officerCode;

                var sql = "exec uspGetPolicyRenewals @OfficerCode";

                DbConnection connection = imisContext.Database.GetDbConnection();

                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;

                    cmd.Parameters.AddRange(new[] { officerCodeParameter });

                    if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                response.Add(new GetPolicyRenewalModel()
                                {
                                    RenewalId = int.Parse(reader["RenewalId"].ToString()),
                                    PolicyId = int.Parse(reader["PolicyId"].ToString()),
                                    OfficerId = int.Parse(reader["OfficerId"].ToString()),
                                    OfficerCode = reader["OfficerCode"].ToString(),
                                    CHFID = reader["CHFID"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    OtherNames = reader["OtherNames"].ToString(),
                                    ProductCode = reader["ProductCode"].ToString(),
                                    ProductName = reader["ProductName"].ToString(),
                                    VillageName = reader["VillageName"].ToString(),
                                    RenewalPromptDate = DateTime.Parse(reader["RenewalPromptDate"].ToStringWithDBNull()),
                                    Phone = reader["Phone"].ToString(),
                                    RenewalUUID = GetRenewalUUIDById(int.Parse(reader["RenewalId"].ToString()))
                                });
                            }
                        } while (reader.NextResult());
                    }
                }
            }

            return response;
        }

        // TODO Add a RV for missing EO or previous policy (currently -5)
        public int Post(PolicyRenewalModel policy)
        {
            int RV = (int)Errors.Renewal.Rejected;

            var policyRenew = policy.GetPolicy();
            var XML = policyRenew.XMLSerialize();

            var fromPhoneRenewalDir = _configuration["AppSettings:FromPhone_Renewal"] + Path.DirectorySeparatorChar;
            var fromPhoneRenewalRejectedDir = _configuration["AppSettings:FromPhone_Renewal_Rejected"] + Path.DirectorySeparatorChar;

            var fileName = "RenPol_" + policy.Date.ToString(DateTimeFormats.FileNameDateTimeFormat) + "_" + policy.CHFID + "_" + policy.ReceiptNo + ".xml";

            var xmldoc = new XmlDocument();
            xmldoc.InnerXml = XML;

            bool ifSaved = false;

            try
            {
                if (!Directory.Exists(fromPhoneRenewalDir)) Directory.CreateDirectory(fromPhoneRenewalDir);
                if (!Directory.Exists(fromPhoneRenewalRejectedDir)) Directory.CreateDirectory(fromPhoneRenewalRejectedDir);

                xmldoc.Save(fromPhoneRenewalDir + fileName);
                ifSaved = true;
            }
            catch
            {
                return (int)Errors.Renewal.UnexpectedException;
            }

            if (ifSaved)
            {
                using (var imisContext = new ImisDB())
                {
                    var xmlParameter = new SqlParameter("@XML", XML) { DbType = DbType.Xml };
                    var returnParameter = OutputParameter.CreateOutputParameter("@RV", SqlDbType.Int);
                    var fileNameParameter = new SqlParameter("@FileName", SqlDbType.NVarChar, 200);
                    fileNameParameter.Value = fileName;

                    var sql = "exec @RV = uspIsValidRenewal @FileName, @XML";

                    DbConnection connection = imisContext.Database.GetDbConnection();

                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;

                        cmd.Parameters.AddRange(new[] { fileNameParameter, xmlParameter, returnParameter });

                        if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Displaying errors in the Stored Procedure in Debug mode
                            do
                            {
                                while (reader.Read())
                                {
                                    Debug.WriteLine("Error/Warning: " + reader.GetValue(0));
                                }
                            } while (reader.NextResult());
                        }
                    }

                    int tempRV = (int)returnParameter.Value;
                    bool moveToRejected = false;

                    switch (tempRV) 
                    {
                        case 0:
                            RV = (int)Errors.Renewal.Accepted;
                            break;
                        case -2:
                            moveToRejected = true;
                            RV = (int)Errors.Renewal.GracePeriodExpired;
                            break;
                        case -4:
                            RV = (int)Errors.Renewal.AlreadyAccepted;
                            break;
                        case -1:
                            moveToRejected = true;
                            RV = (int)Errors.Renewal.UnexpectedException;
                            break;
                        case -5:
                            moveToRejected = true;
                            RV = (int)Errors.Renewal.Rejected;
                            break;
                        default:
                            moveToRejected = true;
                            RV = (int)Errors.Renewal.Rejected;
                            break;
                    }

                    if(moveToRejected)
                    {
                        if (File.Exists(fromPhoneRenewalDir + fileName))
                        {
                            File.Move(fromPhoneRenewalDir + fileName, fromPhoneRenewalRejectedDir + fileName);
                        }
                    }
                }
            }

            if (RV == (int)Errors.Renewal.Accepted)
            {
                RV = UpdateControlNumber(policy);

                CreatePremium(policy);
            }

            return RV;
        }

        public int Delete(Guid uuid)
        {
            int response = 0;

            using (var imisContext = new ImisDB())
            {
                var renewal = imisContext.TblPolicyRenewals.SingleOrDefault(pr => pr.RenewalUUID == uuid);

                if (renewal == null) return -1;

                renewal.ResponseStatus = 2;
                renewal.ResponseDate = DateTime.Now;
                imisContext.SaveChanges();
                response = 1;
            }

            return response;
        }

        private Guid GetRenewalUUIDById(int id)
        {
            Guid response;

            using (var imisContext = new ImisDB())
            {
                response = imisContext.TblPolicyRenewals
                    .Where(o => o.RenewalId == id)
                    .Select(x => x.RenewalUUID)
                    .FirstOrDefault();
            }

            return response;

        }

        public DataMessage GetCommissions(GetCommissionInputs model)
        {
            dynamic response;

            int year = Convert.ToInt32(model.year);
            int month = Convert.ToInt32(model.month);

            // fix - when no month is choosen - get data for every month in given year
            DateTime minDate = new DateTime(year, 1, 1);
            DateTime maxDate = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));

            if (month > 0)
            {
                minDate = new DateTime(year, month, 1);
                maxDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            }

            DataMessage message;

            using (var imisContext = new ImisDB())
            {
                var res = (from PR in imisContext.TblPremium
                           join P in imisContext.TblPolicy.Where(p => p.ValidityTo == null) on PR.PolicyId equals P.PolicyId
                           join R in imisContext.TblReporting on PR.ReportingCommissionID equals R.ReportingId
                           join O in imisContext.TblOfficer on P.OfficerId equals O.OfficerId
                           where ((PR.PayDate >= minDate && PR.PayDate <= maxDate)
                                && PR.ValidityTo == null
                                && O.Code == model.enrolment_officer_code
                                && R.ReportMode == (int)model.mode)
                           select new
                           {
                               Commission = (R.CommissionRate == null ? 0.00M : R.CommissionRate) * PR.Amount,
                               PR.Amount
                           })
                           .ToList();

                var c = res.Count > 0 ? res.Sum(x => x.Commission) : null;
                var a = res.Count > 0 ? res.Sum(x => (decimal?)x.Amount) : null;

                response = new List<dynamic>() { new { Commission = c, Amount = a } };

                message = new GetCommissionResponse(0, false, response, 0).Message;
            }

            return message;
        }

        public int UpdateControlNumber(PolicyRenewalModel renewal)
        {
            if (!String.IsNullOrEmpty(renewal.ControlNumber))
            {

                var sSQL = @"UPDATE PD SET InsuranceNumber = @InsuranceNumber, PolicyStage = N'R', ValidityFrom = GETDATE()
                                FROM tblControlNumber CN
                                INNER JOIN tblPaymentDetails PD ON CN.PaymentId = PD.PaymentID
                                WHERE CN.ValidityTo IS NULL
                                AND CN.ControlNumber = @ControlNumber;";

                SqlParameter[] parameters =
                {
                        new SqlParameter("@ControlNumber", renewal.ControlNumber),
                        new SqlParameter("@InsuranceNumber", renewal.CHFID)
                    };

                try
                {
                    var dh = new DB.SqlServer.DataHelper.DataHelper(_configuration);
                    dh.Execute(sSQL, parameters, CommandType.Text);
                }
                catch (Exception ex)
                {

                    return (int)Errors.Renewal.CouldNotUpdateControlNumber;
                }

                //}
            }

            return (int)Errors.Renewal.Accepted;
        }


        public void CreatePremium(PolicyRenewalModel renewal)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment);
            var paymentLogic = new PaymentLogic(_configuration, _hostingEnvironment);

            if (_configuration.GetValue<bool>("PaymentGateWay:CreatePremiumOnPaymentReceived"))
            {
                int paymentId = payment.GetPaymentId(renewal.ControlNumber);
                _ = paymentLogic.CreatePremium(paymentId);
            }

        }

        public async Task<DataMessage> SelfRenewal(SelfRenewal renewal)
        {
            var helper = new SelfRenewalHelper(_configuration, _hostingEnvironment);
            var response = await helper.CreateSelfRenewal(renewal);
            
            return response;
        }
    }
}
