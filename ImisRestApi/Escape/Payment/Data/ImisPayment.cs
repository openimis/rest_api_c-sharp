using ImisRestApi.Escape.Payment.Models;
using ImisRestApi.Data;
using ImisRestApi.Extensions;
using ImisRestApi.Models;
using ImisRestApi.Models.Payment;
using ImisRestApi.Models.Payment.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ImisRestApi.Data
{
    public class ImisPayment : ImisBasePayment
    {
        private IHostingEnvironment env;
        private IConfiguration config;

        public ImisPayment(IConfiguration configuration, IHostingEnvironment hostingEnvironment) : base(configuration, hostingEnvironment)
        {
            env = hostingEnvironment;
            config = configuration;
        }

#if CHF
        public object RequestReconciliationReport(int daysAgo, String productSPCode)
        {
            daysAgo = -1 * daysAgo;

            GepgUtility gepg = new GepgUtility(_hostingEnvironment,config);

            ReconcRequest Reconciliation = new ReconcRequest();

            gepgSpReconcReq request = new gepgSpReconcReq();
            request.SpReconcReqId = Math.Abs(Guid.NewGuid().GetHashCode());//Convert.ToInt32(DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString());
            request.SpCode = productSPCode;
            request.SpSysId = Configuration["PaymentGateWay:GePG:SystemId"];
            request.TnxDt = DateTime.Now.AddDays(daysAgo).ToString("yyyy-MM-dd");
            request.ReconcOpt = 1;

            var requestString = gepg.SerializeClean(request, typeof(gepgSpReconcReq));
            string signature = gepg.GenerateSignature(requestString);
            var signedRequest = gepg.FinaliseSignedMsg(new ReconcRequest() { gepgSpReconcReq = request, gepgSignature = signature }, typeof(ReconcRequest));

            var result = gepg.SendReconcHttpRequest(signedRequest, productSPCode);

            return new { reconcId = request.SpReconcReqId, resp = result };

        }

        public override decimal determineTransferFee(decimal expectedAmount, TypeOfPayment typeOfPayment)
        {
            if (typeOfPayment == TypeOfPayment.BankTransfer || typeOfPayment == TypeOfPayment.Cash) {
                return 0;
            }
            else
            {
                var fee = expectedAmount - (expectedAmount / Convert.ToDecimal(1.011));

                return Math.Round(fee,0);
            }
        }

        public override decimal determineTransferFeeReverse(decimal expectedAmount, TypeOfPayment typeOfPayment)
        {
            if (typeOfPayment == TypeOfPayment.BankTransfer || typeOfPayment == TypeOfPayment.Cash)
            {
                return 0;
            }
            else
            {
                var fee = expectedAmount * Convert.ToDecimal(0.011);
                return Math.Round(fee, 0);
            }
        }

        public override decimal GetToBePaidAmount(decimal ExpectedAmount, decimal TransferFee)
        {
            decimal amount = ExpectedAmount - TransferFee;
            return Math.Round(amount, 0);
        }

        public override PostReqCNResponse PostReqControlNumber(string OfficerCode, string PaymentId, string PhoneNumber, decimal ExpectedAmount, List<PaymentDetail> products, string controlNumber = null, bool acknowledge = false, bool error = false)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment,config);
            var bill = gepg.CreateBill(Configuration, OfficerCode, PhoneNumber, PaymentId, Math.Round(ExpectedAmount,2), InsureeProducts);
            var signature = gepg.GenerateSignature(bill);

            var signedMesg = gepg.FinaliseSignedMsg(signature);
            var billAck = gepg.SendHttpRequest(signedMesg, InsureeProducts);

            string mydocpath = System.IO.Path.Combine(env.WebRootPath, "controlNumberAck");
            string namepart = new Random().Next(100000, 999999).ToString();

            string reconc = JsonConvert.SerializeObject(billAck);
            string sentbill = JsonConvert.SerializeObject(bill);

            var content = sentbill + "********************" + reconc;
            var gepgFile = new GepgFoldersCreating(PaymentId, "CN_Request", content, env);
            gepgFile.putToTargetFolderPayment();

            return base.PostReqControlNumber(OfficerCode, PaymentId, PhoneNumber, ExpectedAmount, products, null, true, false);
        }

        public string ControlNumberResp(int code)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment,config);

            gepgBillSubRespAck CnAck = new gepgBillSubRespAck();
            CnAck.TrxStsCode = code;

            var CnAckString = gepg.SerializeClean(CnAck, typeof(gepgBillSubRespAck));
            string signature = gepg.GenerateSignature(CnAckString);
            var signedCnAck = gepg.FinaliseSignedAcks(new GepgBillResponseAck() { gepgBillSubRespAck = CnAck, gepgSignature = signature }, typeof(GepgBillResponseAck));

            return signedCnAck;
        }

        public string PaymentResp(int code)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment,config);

            gepgPmtSpInfoAck PayAck = new gepgPmtSpInfoAck();
            PayAck.TrxStsCode = code;

            var PayAckString = gepg.SerializeClean(PayAck, typeof(gepgPmtSpInfoAck));
            string signature = gepg.GenerateSignature(PayAckString);
            var signedPayAck = gepg.FinaliseSignedAcks(new GepgPaymentAck() { gepgPmtSpInfoAck = PayAck, gepgSignature = signature }, typeof(GepgPaymentAck));

            return signedPayAck;
        }

        public string ReconciliationResp(int code)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment,config);

            gepgSpReconcRespAck ReconcAck = new gepgSpReconcRespAck();
            ReconcAck.ReconcStsCode = code;

            var ReconcAckString = gepg.SerializeClean(ReconcAck, typeof(gepgSpReconcRespAck));
            string signature = gepg.GenerateSignature(ReconcAckString);
            var signedReconcAck = gepg.FinaliseSignedAcks(new GepgReconcRespAck() { gepgSpReconcRespAck = ReconcAck, gepgSignature = signature }, typeof(GepgReconcRespAck));

            return signedReconcAck;
        }

        public bool IsValidCall(object Reqbody,int callNo) {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment,config);

            var _body = GetXmlStringFromObject(Reqbody);
            var body = _body.Replace(" />","/>");
            var content = string.Empty;
            var signature = string.Empty;
            switch (callNo)
            {
                case 0:
                    content = gepg.getContent(body, "gepgBillSubResp");
                    signature = gepg.getSig(body, "gepgSignature");
                    break;
                case 1:
                    content = gepg.getContent(body, "gepgPmtSpInfo");
                    signature = gepg.getSig(body, "gepgSignature");
                    break;
                case 2:
                    content = gepg.getContent(body, "gepgSpReconcResp");
                    signature = gepg.getSig(body, "gepgSignature");
                    break;
                default:
                    break;
            }

            return gepg.VerifyData(content, signature);
        }

        public bool IsCallValid(string Reqbody, int callNo)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment, config);

            var body = Reqbody;
            var content = string.Empty;
            var signature = string.Empty;
            switch (callNo)
            {
                case 0:
                    content = gepg.getContent(body, "gepgBillSubResp");
                    signature = gepg.getSig(body, "gepgSignature");
                    break;
                case 1:
                    content = gepg.getContent(body, "gepgPmtSpInfo");
                    signature = gepg.getSig(body, "gepgSignature");
                    break;
                case 2:
                    content = gepg.getContent(body, "gepgSpReconcResp");
                    signature = gepg.getSig(body, "gepgSignature");
                    break;
                default:
                    break;
            }

            return gepg.VerifyData(content, signature);
        }

        private string GetXmlStringFromObject(object obj)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, obj);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }

            return sw.ToString();
        }

#endif
    }
}
