using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Models;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Repo
{
    public class PaymentRepo : Connection
    {
        public string PaymentId { get; set; }
        public float ExpectedAmount { get; set; }

        private IntentOfPay _intent { get; set; }

        public PaymentRepo(IConfiguration configuration) :base(configuration)
        {

        }

        public PaymentRepo(IConfiguration configuration,IntentOfPay Intent) : base(configuration)
        {
            _intent = Intent;
        }

        public void SaveIntent()
        {
            XElement PaymentIntent = new XElement("PaymentIntent",
                new XElement("Header",
                    new XElement("OfficerCode", _intent.OfficerCode),
                    new XElement("RequestDate", _intent.RequestDate.ToShortDateString()),
                    new XElement("PhoneNumber", _intent.PhoneNumber),
                    new XElement("AuditUserId", -1)
                ),
                  new XElement("Details",
                               new XElement("Detail",
                                  new XElement("InsureeNumber", _intent.InsureeNumber),
                                  new XElement("ProductCode", _intent.ProductCode),
                                  new XElement("EnrollmentDate", DateTime.UtcNow),
                                  new XElement("IsRenewal", _intent.IsRenewal()))
                                  )
                              );
             // );


            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
                new SqlParameter("@PaymentID", SqlDbType.Int){Direction = ParameterDirection.Output },
                new SqlParameter("@ExpectedAmount", SqlDbType.Int){Direction = ParameterDirection.Output }
             };

            try
            {
                var data = Procedure("uspInsertPaymentIntent", sqlParameters);

                PaymentId = data[0].Value.ToString();
                ExpectedAmount = float.Parse(data[1].Value.ToString());
            }
            catch (Exception e)
            {

                throw new Exception();
            }
            

        }

        public void SaveControlNumber()
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", PaymentId),
                new SqlParameter("@RequestOrigin", "IMIS")
             };

            var data = Procedure("uspRequestGetControlNumber", sqlParameters);
        }

        public void UpdateControlNumberStatus(string ControlNumber,CnStatus status){

            SqlParameter[] sqlParameters = {
                  new SqlParameter("@ControlNumber", ControlNumber)
            };

            switch (status)
            {
                case CnStatus.Sent:
                    break;
                case CnStatus.Acknowledged:
                    Procedure("uspAcknowledgeControlNumber", sqlParameters);
                    break;
                case CnStatus.Issued:                 
                    Procedure("uspIssueControlNumber", sqlParameters);
                    break;
                case CnStatus.Paid:
                    Procedure("uspPaidControlNumber", sqlParameters);
                    break;
                case CnStatus.Rejected:
                    break;
                default:
                    break;
            }
        }

        public void SaveControlNumber(string ControlNumber)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", PaymentId),
                new SqlParameter("@RequestOrigin", "IMIS"),
                new SqlParameter("@ControlNumber", "IMIS")
             };

            var data = Procedure("uspReceiveControlNumber", sqlParameters);
        }

        public void SaveControlNumberAkn(bool Success, string Comment)
        {
            XElement CNAcknowledgement = new XElement("ControlNumberAcknowledge",
                  new XElement("PaymentID", PaymentId),
                  new XElement("Success", Success),
                  new XElement("Comment", Comment)
                  );

            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", CNAcknowledgement)
             };

            var data = Procedure("uspAcknowledgeControlNumberRequest", sqlParameters);
        }

        public void SavePayment(PaymentModels payment)
        {
            XElement PaymentIntent = new XElement("PaymentData",
                new XElement("PaymentID", PaymentId),
                   new XElement("ControlNumber", payment.ControlNumber),
                   new XElement("Amount", payment.ReceivedAmount),
                   new XElement("InsureeNumber", payment.InsureeNumber)
                             );


            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
             };
            var data = Procedure("uspReceivePayment", sqlParameters);

        }

        public bool Valid(string InsureeNumber,string ProductCode)
        {
            return false;
        }
    }
}
