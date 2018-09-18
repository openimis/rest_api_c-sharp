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


        public string GenerateCtrlNoRequest(string OfficerCode,string BillId, double ExpectedAmount, List<PaymentDetail> products)
        {

            GepgUtility gepg = new GepgUtility(_hostingEnvironment);
            var bill = gepg.CreateBill(Configuration, OfficerCode, BillId, ExpectedAmount, products);
            var signature = gepg.GenerateSignature(bill);

           
            var signedMesg = gepg.FinaliseSignedMsg(signature);
            var billAck = gepg.SendHttpRequest(signedMesg);

            SqlParameter[] sqlParameters = {
                new SqlParameter("@PaymentID", BillId)
             };

            try
            {
                var data = dh.ExecProcedure("uspRequestGetControlNumber", sqlParameters);

            }
            catch (Exception e)
            {

                throw new Exception();
            }

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
                    _intent.PaymentDetails.Select(x =>
                                    
                               new XElement("Detail",
                                  new XElement("InsuranceNumber", x.InsureeNumber),
                                  new XElement("ProductCode", x.ProductCode),
                                  new XElement("IsRenewal", x.IsRenewal())
                                  )                      
                    )
                  )
            );
            // );


            SqlParameter[] sqlParameters = {
                new SqlParameter("@Xml", PaymentIntent.ToString()),
                new SqlParameter("@PaymentID", SqlDbType.Int){Direction = ParameterDirection.Output },
                new SqlParameter("@ExpectedAmount", SqlDbType.Int){Direction = ParameterDirection.Output },
                new SqlParameter("@RV",SqlDbType.Int){Direction = ParameterDirection.ReturnValue }
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

        public string RequestReconciliationReport()
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment);

            ReconcRequest Reconciliation = new ReconcRequest();

            gepgSpReconcReq request = new gepgSpReconcReq();
            request.SpReconcReqId = Convert.ToInt32(DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString());
            request.SpCode = Configuration["PaymentGateWay:GePG:SpCode"];
            request.SpSysId = Configuration["PaymentGateWay:GePG:SystemId"];
            request.TnxDt = DateTime.UtcNow.Date;
            request.ReconcOpt = 2;

            var requestString = gepg.SerializeClean(request, typeof(gepgSpReconcReq));
            string signature = gepg.GenerateSignature(requestString);
            var signedRequest = gepg.FinaliseSignedMsg(new ReconcRequest() { gepgSpReconcReq = request, gepgSignature = signature }, typeof(ReconcRequest));

            var result = gepg.SendHttpRequest(signedRequest);

            return result;

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
