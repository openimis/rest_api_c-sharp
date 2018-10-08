using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Data;
using ImisRestApi.Models;
using ImisRestApi.Models.Payment;
using ImisRestApi.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImisRestApi.Data
{
    public class ImisBasePayment
    {
        public string PaymentId { get; set; }
        public decimal ExpectedAmount { get; set; }
        public string ControlNum { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime? PaymentDate { get; set; }
        public decimal? PaidAmount { get; set; }
        public List<InsureeProduct> InsureeProducts { get; set; }

        protected IConfiguration Configuration;
        protected readonly IHostingEnvironment _hostingEnvironment;
        protected DataHelper dh;

        public ImisBasePayment(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            dh = new DataHelper(configuration);
        }

        public bool SaveControlNumberRequest(string BillId,bool failed)
        {

            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", BillId),
                new SqlParameter("@Failed", failed)
             };

            try
            {
                var data = dh.ExecProcedure("uspRequestGetControlNumber", sqlParameters);
                GetPaymentInfo(BillId);
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public virtual ControlNumberResp GenerateCtrlNoRequest(string OfficerCode, string PaymentId, decimal ExpectedAmount, List<PaymentDetail> products,string controlNumber = null,bool acknowledge = false,bool error = false)
        {
            bool result = SaveControlNumberRequest(PaymentId,error);
            string ctrlNumber = null;

            //BEGIN Temporary Control Number Generator(Simulation For Testing Only)
            var randomNumber = new Random().Next(100000, 999999);
            
            if(randomNumber%2 == 0)
            {
                ctrlNumber = randomNumber.ToString();
            }
            //END Temporary 

            ControlNumberResp response = new ControlNumberResp() {
                PaymentId = PaymentId,
                ControlNumber = ctrlNumber,
                RequestAcknowledged = acknowledge,
                ErrorMessage = "",
                ErrorOccured = error
            };

            return response;
        }

        public DataMessage SaveIntent(IntentOfPay _intent, int? errorNumber = 0, string errorMessage = null)
        {
            XElement PaymentIntent = new XElement("PaymentIntent",
                    new XElement("Header",
                        new XElement("OfficerCode", _intent.OfficerCode),
                        new XElement("RequestDate",_intent.RequestDate),
                        new XElement("PhoneNumber", _intent.PhoneNumber),
                        new XElement("AuditUserId", -1)
                    ),
                      new XElement("Details",
                    _intent.PaymentDetails.Select(x =>

                               new XElement("Detail",
                                  new XElement("InsuranceNumber", x.InsureeNumber),
                                  new XElement("ProductCode", x.ProductCode),
                                  new XElement("EnrollmentDate", DateTime.UtcNow),
                                  new XElement("IsRenewal", x.IsRenewal())
                                  )
                    )
                  ),
                   new XElement("ProxySettings",
                        new XElement("AdultMembers",Configuration["DefaultFamily:Adults"]),
                        new XElement("ChildMembers", Configuration["DefaultFamily:Children"])
                   )
            );



            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
                new SqlParameter("@ErrorNumber",errorNumber),
                new SqlParameter("@ErrorMsg",errorMessage),
                new SqlParameter("@PaymentID", SqlDbType.Int){Direction = ParameterDirection.Output },
                new SqlParameter("@ExpectedAmount", SqlDbType.Decimal){Direction = ParameterDirection.Output }
             };

            DataMessage message;

            try
            {
                bool error = true;
                var data = dh.ExecProcedure("uspInsertPaymentIntent", sqlParameters);

                var rv = int.Parse(data[2].Value.ToString());

                if (rv == 0)
                { 
                    error = false;
                }

                PaymentId = data[0].Value.ToString();
                ExpectedAmount = decimal.Parse(data[1].Value.ToString());
                message = new SaveIntentResponse(rv, error).Message;
                GetPaymentInfo(PaymentId);
            }
            catch (Exception e)
            {

                message = new SaveIntentResponse(e).Message;
            }

            return message;
        }

        public DataMessage SaveControlNumber(string ControlNumber,bool failed)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", PaymentId),
                new SqlParameter("@ControlNumber", ControlNumber),
                new SqlParameter("@Failed", failed)
             };

            DataMessage message;

            try
            {
                var data = dh.ExecProcedure("uspReceiveControlNumber", sqlParameters);
                message = new CtrlNumberResponse(int.Parse(data[0].Value.ToString()), false).Message;
                GetPaymentInfo(PaymentId);
            }
            catch (Exception e)
            {

                message = new CtrlNumberResponse(e).Message;
            }

            return message;

        }

        public DataMessage SaveControlNumber(ControlNumberResp model,bool failed)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", model.PaymentId),
                new SqlParameter("@ControlNumber", model.ControlNumber),
                new SqlParameter("@Failed", failed)
             };

            DataMessage message;

            try
            {
                var data = dh.ExecProcedure("uspReceiveControlNumber", sqlParameters);
                message = new CtrlNumberResponse(int.Parse(data[0].Value.ToString()), false).Message;
                GetPaymentInfo(model.PaymentId);
            }
            catch (Exception e)
            {

                message = new CtrlNumberResponse(e).Message;
            }

            return message;
        }

        public bool CheckControlNumber(string PaymentID, string ControlNumber)
        {
            var sSQL = @"SELECT * FROM tblControlNumber WHERE PaymentID != @PaymentID AND ControlNumber = @ControlNumber";

            SqlParameter[] parameters = {
                new SqlParameter("@PaymentID", PaymentID),
                new SqlParameter("@ControlNumber", ControlNumber)
            };
            bool result = false;

            try
            {
                var data = dh.GetDataTable(sSQL, parameters, CommandType.Text);
                if (data.Rows.Count > 0)
                {
                    result = true;
                }
                GetPaymentInfo(PaymentID);
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public void UpdateControlNumberStatus(string ControlNumber, CnStatus status)
        {

            SqlParameter[] sqlParameters = {
                  new SqlParameter("@ControlNumber", ControlNumber)
            };
             
            switch (status)
            {
                case CnStatus.Sent:
                    break;
                case CnStatus.Acknowledged:
                    dh.ExecProcedure("uspAcknowledgeControlNumber", sqlParameters);
                    break;
                case CnStatus.Issued:
                    dh.ExecProcedure("uspIssueControlNumber", sqlParameters);
                    break;
                case CnStatus.Paid:
                    dh.ExecProcedure("uspPaidControlNumber", sqlParameters);
                    break;
                case CnStatus.Rejected:
                    break;
                default:
                    break;
            }
        }
      
        public DataMessage SaveControlNumberAkn(bool Success, string Comment)
        {
            XElement CNAcknowledgement = new XElement("ControlNumberAcknowledge",
                  new XElement("PaymentID", PaymentId),
                  new XElement("Success", Convert.ToInt32(Success)),
                  new XElement("Comment", Comment)
                  );

            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml",CNAcknowledgement.ToString())
             };


            DataMessage message;

            try
            {
                var data = dh.ExecProcedure("uspAcknowledgeControlNumberRequest", sqlParameters);
                message = new SaveAckResponse(int.Parse(data[0].Value.ToString()), false).Message;
                GetPaymentInfo(PaymentId);
            }
            catch (Exception e)
            {

                message = new SaveAckResponse(e).Message;
            }

            return message;

        }

        public DataMessage SavePayment(PaymentData payment,bool failed = false)
        {
            int? isRenewal = null;

            if((payment.PaymentType != null))
            {
                isRenewal = (int)payment.PaymentType;
            }
            
            XElement PaymentIntent = new XElement("PaymentData",
                new XElement("PaymentID", payment.PaymentId),
                new XElement("PaymentDate", payment.PaymentDate),
                new XElement("ControlNumber", payment.ControlNumber),
                new XElement("Amount", payment.ReceivedAmount),
                new XElement("ReceiptNo", payment.ReceiptNumber),
                new XElement("TransactionNo", payment.TransactionId),
                new XElement("PhoneNumber", payment.PhoneNumber),
                new XElement("PaymentOrigin", payment.PaymentOrigin),
                new XElement("OfficerCode", payment.EnrolmentOfficerCode),
                new XElement("Detail",
                    new XElement("InsureeNumber", payment.InsureeNumber),
                    new XElement("ProductCode", payment.ProductCode),
                    new XElement("IsRenewal", isRenewal)
                            )
               );


            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
             };
            
            DataMessage message;

            try
            {
                var data = dh.ExecProcedure("uspReceivePayment", sqlParameters);
                message = new SavePayResponse(int.Parse(data[0].Value.ToString()), false).Message;
                GetPaymentInfo(payment.PaymentId);
            }
            catch (Exception e)
            {
                message = new SavePayResponse(e).Message;
            }

            return message;

        }

        public DataMessage Match(MatchModel model)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", model.PaymentId),
                new SqlParameter("@AuditUserId", model.AuditUserId)
             };

            DataMessage message;

            try
            {
                DataSet data = dh.FillDataSet("uspMatchPayment", sqlParameters, CommandType.StoredProcedure);
                DataTable dt = data.Tables[data.Tables.Count - 1];

                bool error = true;

                if (dt.Rows.Count > 0) {
                    var firstRow = dt.Rows[0];

                    if(Convert.ToInt32(firstRow["PaymentMatched"]) > 0)
                    {
                        error = false;
                    }
                    
                }
                else
                {
                    error = true;
                }

                
                message = new ImisApiResponse(0,error,dt).Message;
                GetPaymentInfo(model.PaymentId.ToString());
            }
            catch (Exception e)
            {
                message = new ImisApiResponse(e).Message;
            }

            return message;
        }

        public void GetPaymentInfo(string Id)
        {
            var sSQL = @"SELECT tblPayment.PaymentID, tblPayment.ExpectedAmount, tblPaymentDetails.ExpectedAmount AS ExpectedDetailAmount,
                        tblPayment.ReceivedAmount, tblPayment.PaymentDate, tblInsuree.LastName, tblInsuree.OtherNames,tblPaymentDetails.InsuranceNumber,tblPayment.PhoneNumber,
                        tblProduct.ProductName, tblPaymentDetails.ProductCode, tblPolicy.ExpiryDate, tblPolicy.EffectiveDate,tblControlNumber.ControlNumber,tblPolicy.PolicyStatus
                        FROM tblControlNumber 
                        RIGHT OUTER JOIN tblInsuree 
                        RIGHT OUTER JOIN tblProduct 
                        RIGHT OUTER JOIN tblPayment 
                        INNER JOIN tblPaymentDetails 
                        ON tblPayment.PaymentID = tblPaymentDetails.PaymentID 
                        ON tblProduct.ProductCode = tblPaymentDetails.ProductCode 
                        ON tblInsuree.CHFID = tblPaymentDetails.InsuranceNumber 
                        ON tblControlNumber.PaymentID = tblPayment.PaymentID 
                        LEFT OUTER JOIN tblPremium 
                        LEFT OUTER JOIN tblPolicy 
                        ON tblPremium.PolicyID = tblPolicy.PolicyID 
                        ON tblPaymentDetails.PremiumID = tblPremium.PremiumId
                        WHERE (tblPayment.PaymentID = @PaymentID) AND (tblProduct.ValidityTo IS NULL) AND (tblInsuree.ValidityTo IS NULL)";

            SqlParameter[] parameters = {
                new SqlParameter("@PaymentID", Id)
            };

            try
            {
                var data = dh.GetDataTable(sSQL, parameters, CommandType.Text);
               
                if (data.Rows.Count > 0)
                {
                    var row1 = data.Rows[0];
                    PaymentId = Id;
                    ControlNum = Convert.ToString(row1["ControlNumber"]);
                    ExpectedAmount = Convert.ToDecimal(row1["ExpectedAmount"]);
                    PhoneNumber = Convert.ToString(row1["PhoneNumber"]);
                    PaymentDate = (DateTime?)(row1["PaymentDate"] != System.DBNull.Value ? row1["PaymentDate"] : null);
                    PaidAmount = (decimal?)(row1["ReceivedAmount"] != System.DBNull.Value ? row1["ReceivedAmount"] : null);
                    InsureeProducts = new List<InsureeProduct>();

                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        var rw = data.Rows[i];

                        bool active = false;

                        if (rw["PolicyStatus"] != System.DBNull.Value && Convert.ToInt32(rw["PolicyStatus"]) == 1) {
                            active = true;
                        }

                        InsureeProducts.Add(
                                new InsureeProduct()
                                {
                                    InsureeNumber = Convert.ToString("InsuranceNumber"),
                                    InsureeName = Convert.ToString(rw["OtherNames"]) + Convert.ToString(rw["LastName"]),
                                    ProductName = Convert.ToString(rw["ProductName"]),
                                    ProductCode = Convert.ToString(rw["ProductCode"]),
                                    ExpiryDate = (DateTime?)(rw["ExpiryDate"] != System.DBNull.Value?rw["ExpiryDate"] :null),
                                    EffectiveDate = (DateTime?)(rw["EffectiveDate"] != System.DBNull.Value ? rw["EffectiveDate"] : null),
                                    PolicyActivated = active
                                }
                            );
                        
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
