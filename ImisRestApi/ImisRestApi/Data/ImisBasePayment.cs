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
        public float ExpectedAmount { get; set; }

        protected IConfiguration Configuration;
        protected readonly IHostingEnvironment _hostingEnvironment;
        protected DataHelper dh;

        public ImisBasePayment(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            dh = new DataHelper(configuration);
        }

        public bool SaveControlNumberRequest(string BillId)
        {

            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", BillId)
             };

            try
            {
                var data = dh.ExecProcedure("uspRequestGetControlNumber", sqlParameters);

            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public virtual ControlNumberResp GenerateCtrlNoRequest(string OfficerCode, string PaymentId, double ExpectedAmount, List<PaymentDetail> products)
        {
            bool result = SaveControlNumberRequest(PaymentId);

            ControlNumberResp response = new ControlNumberResp()
            {
                PaymentId = PaymentId,
                ControlNumber = "",
                ErrorMessage = "",
                ErrorOccured = false
            };

            return response;
        }

        public DataMessage SaveIntent(IntentOfPay _intent)
        {
            XElement PaymentIntent = new XElement("PaymentIntent",
                    new XElement("Header",
                        new XElement("OfficerCode", _intent.OfficerCode),
                        new XElement("RequestDate", _intent.RequestDate.ToShortDateString()),
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
                  )
            );
            // );


            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
                new SqlParameter("@PaymentID", SqlDbType.Int){Direction = ParameterDirection.Output },
                new SqlParameter("@ExpectedAmount", SqlDbType.Int){Direction = ParameterDirection.Output }
             };

            DataMessage message;

            try
            {
                var data = dh.ExecProcedure("uspInsertPaymentIntent", sqlParameters);

                PaymentId = data[0].Value.ToString();
                ExpectedAmount = float.Parse(data[1].Value.ToString());

                message = new SaveIntentResponse(int.Parse(data[2].Value.ToString()), false).Message;
            }
            catch (Exception e)
            {

                message = new SaveIntentResponse(e).Message;
            }

            return message;
        }

        public DataMessage SaveControlNumber()
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", PaymentId),
                new SqlParameter("@RequestOrigin", "IMIS")
             };

            DataMessage message;

            try
            {
                var data = dh.ExecProcedure("uspRequestGetControlNumber", sqlParameters);
                message = new ImisApiResponse(int.Parse(data[0].Value.ToString()), false).Message;
            }
            catch (Exception e)
            {
                message = new ImisApiResponse(e).Message;
            }

            return message;
        }

        public DataMessage SaveControlNumber(string ControlNumber)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", PaymentId),
                new SqlParameter("@ControlNumber", ControlNumber)
             };

            DataMessage message;

            try
            {
                var data = dh.ExecProcedure("uspReceiveControlNumber", sqlParameters);
                message = new ImisApiResponse(int.Parse(data[0].Value.ToString()), false).Message;
            }
            catch (Exception e)
            {

                message = new ImisApiResponse(e).Message;
            }

            return message;

        }

        public DataMessage SaveControlNumber(ControlNumberResp model)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", model.PaymentId),
                new SqlParameter("@ControlNumber", model.ControlNumber)
             };

            DataMessage message;

            try
            {
                var data = dh.ExecProcedure("uspReceiveControlNumber", sqlParameters);
                message = new ImisApiResponse(int.Parse(data[0].Value.ToString()), false).Message;
            }
            catch (Exception e)
            {

                message = new ImisApiResponse(e).Message;
            }

            return message;
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
                message = new ImisApiResponse(int.Parse(data[0].Value.ToString()), false).Message;
            }
            catch (Exception e)
            {

                message = new ImisApiResponse(e).Message;
            }

            return message;

        }

        public DataMessage SavePayment(PaymentData payment)
        {
            XElement PaymentIntent = new XElement("PaymentData",
                new XElement("PaymentDate", payment.PaymentDate),
                new XElement("ControlNumber", payment.ControlNumber),
                new XElement("Amount", payment.ReceivedAmount),
                new XElement("ReceiptNo", payment.ReceiptNumber),
                new XElement("TransactionNo", payment.TransactionId),
                new XElement("PhoneNumber", payment.PhoneNumber),
                new XElement("InsureeNumber", payment.InsureeNumber)
                             );


            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
             };

            DataMessage message;

            try
            {
                var data = dh.ExecProcedure("uspReceivePayment", sqlParameters);
                message = new ImisApiResponse(int.Parse(data[0].Value.ToString()), false).Message;
            }
            catch (Exception e)
            {
                message = new ImisApiResponse(e).Message;
            }

            return message;

        }

        public bool Valid(string InsureeNumber, string ProductCode)
        {
            return false;
        }
    }
}
