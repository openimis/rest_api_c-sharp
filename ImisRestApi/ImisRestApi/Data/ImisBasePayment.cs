using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Data;
using ImisRestApi.Models;
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

        public bool SaveControlNumberRequest(string BillId) {

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

        public virtual int GenerateCtrlNoRequest(string OfficerCode,string BillId, double ExpectedAmount, List<PaymentDetail> products)
        {
            bool result = SaveControlNumberRequest(BillId);

            return 1;
        }

        public void SaveIntent(IntentOfPay _intent)
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

            try
            {
                var data = dh.ExecProcedure("uspInsertPaymentIntent", sqlParameters);

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

            var data = dh.ExecProcedure("uspRequestGetControlNumber", sqlParameters);
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

        public void SaveControlNumber(string ControlNumber)
        {
            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", PaymentId),
                new SqlParameter("@ControlNumber", ControlNumber)
             };

            var data = dh.ExecProcedure("uspReceiveControlNumber", sqlParameters);
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

            var data = dh.ExecProcedure("uspAcknowledgeControlNumberRequest", sqlParameters);
        }
      
        public void SavePayment(PymtTrxInf payment)
        {
            XElement PaymentIntent = new XElement("PaymentData",
                new XElement("PaymentDate", payment.PaymentDate),
                new XElement("ControlNumber", payment.PayCtrNum),
                new XElement("Amount", payment.PaidAmt),
                new XElement("ReceiptNo", payment.PspReceiptNumber),
                new XElement("TransactionNo",payment.TrxId),
                new XElement("PhoneNumber",payment.InsureeNumber),
                new XElement("InsureeNumber", payment.InsureeNumber)
                             );


            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
             };
            var data = dh.ExecProcedure("uspReceivePayment", sqlParameters);

        }

        public bool Valid(string InsureeNumber, string ProductCode)
        {
            return false;
        }
    }
}
