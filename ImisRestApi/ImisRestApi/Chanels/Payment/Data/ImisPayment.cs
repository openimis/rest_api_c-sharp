using ImisRestApi.Chanels.Payment.Data;
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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImisRestApi.Data
{
    public class ImisPayment
    {
        public string PaymentId { get; set; }
        public float ExpectedAmount { get; set; }

        private IConfiguration Configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private DataHelper dh;

        public ImisPayment(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            dh = new DataHelper(configuration);
        }


        public string GenerateCtrlNoRequest(string OfficerCode, string InsureeNumber,string BillId, double ExpectedAmount, List<PaymentDetail> products)
        {

            GepgUtility gepg = new GepgUtility(_hostingEnvironment);
            var bill = gepg.CreateBill(Configuration, OfficerCode, InsureeNumber, BillId, ExpectedAmount, products);
            var signature = gepg.GenerateSignature(bill);

           
            var signedMesg = gepg.FinaliseSignedMsg(signature);
            var billAck = gepg.SendHttpRequest(signedMesg);
            return billAck;
          
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
                new SqlParameter("@RequestOrigin", "IMIS"),
                new SqlParameter("@ControlNumber", "IMIS")
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
                new XElement("PaymentID", PaymentId),
                   new XElement("ControlNumber", payment.PayCtrNum),
                   new XElement("Amount", payment.PaidAmt),
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

        public string ControlNumberResp() {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment);

            gepgBillSubRespAck CnAck = new gepgBillSubRespAck();
            CnAck.TrxStsCode = 7101;

            var CnAckString = gepg.SerializeClean(CnAck, typeof(gepgBillSubRespAck));
            string signature = gepg.GenerateSignature(CnAckString);
            var signedCnAck = gepg.FinaliseSignedMsg(new GepgBillResponseAck() { gepgBillSubRespAck = CnAck, gepgSignature = signature}, typeof(GepgBillResponseAck));

            return signedCnAck;
        }

        public string PaymentResp()
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment);

            gepgPmtSpInfoAck PayAck = new gepgPmtSpInfoAck();
            PayAck.TrxStsCode = 7101;

            var PayAckString = gepg.SerializeClean(PayAck, typeof(gepgPmtSpInfoAck));
            string signature = gepg.GenerateSignature(PayAckString);
            var signedPayAck = gepg.FinaliseSignedMsg(new GepgPaymentAck() { gepgPmtSpInfoAck = PayAck, gepgSignature = signature}, typeof(GepgPaymentAck));

            return signedPayAck;
        }

        public string ReconciliationResp()
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment);

            gepgSpReconcReqAck ReconcAck = new gepgSpReconcReqAck();
            ReconcAck.ReconcStsCode = 7101;

            var ReconcAckString = gepg.SerializeClean(ReconcAck, typeof(gepgSpReconcReqAck));
            string signature = gepg.GenerateSignature(ReconcAckString);
            var signedReconcAck = gepg.FinaliseSignedMsg(new GepgReconcAck() { gepgSpReconcReqAck = ReconcAck, gepgSignature = signature }, typeof(GepgReconcAck));

            return signedReconcAck;
        }
    }
}
