using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Data;
using ImisRestApi.Models;
using ImisRestApi.Models.Payment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Data
{
    public class ImisPayment:ImisBasePayment
    {
        public ImisPayment(IConfiguration configuration, IHostingEnvironment hostingEnvironment) :base(configuration, hostingEnvironment)
        {

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

        public override ControlNumberResp PostReqControlNumber(string OfficerCode, string PaymentId, decimal ExpectedAmount, List<PaymentDetail> products, string controlNumber = null, bool acknowledge = false, bool error = false)
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment);
            var bill = gepg.CreateBill(Configuration, OfficerCode, PaymentId, ExpectedAmount, products);
            var signature = gepg.GenerateSignature(bill);

            var signedMesg = gepg.FinaliseSignedMsg(signature);
            var billAck = gepg.SendHttpRequest(signedMesg);

            return base.PostReqControlNumber(OfficerCode, PaymentId, ExpectedAmount, products,null,true,false);
        }

        public string ControlNumberResp()
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment);

            gepgBillSubRespAck CnAck = new gepgBillSubRespAck();
            CnAck.TrxStsCode = 7101;

            var CnAckString = gepg.SerializeClean(CnAck, typeof(gepgBillSubRespAck));
            string signature = gepg.GenerateSignature(CnAckString);
            var signedCnAck = gepg.FinaliseSignedMsg(new GepgBillResponseAck() { gepgBillSubRespAck = CnAck, gepgSignature = signature }, typeof(GepgBillResponseAck));

            return signedCnAck;
        }

        public string PaymentResp()
        {
            GepgUtility gepg = new GepgUtility(_hostingEnvironment);

            gepgPmtSpInfoAck PayAck = new gepgPmtSpInfoAck();
            PayAck.TrxStsCode = 7101;

            var PayAckString = gepg.SerializeClean(PayAck, typeof(gepgPmtSpInfoAck));
            string signature = gepg.GenerateSignature(PayAckString);
            var signedPayAck = gepg.FinaliseSignedMsg(new GepgPaymentAck() { gepgPmtSpInfoAck = PayAck, gepgSignature = signature }, typeof(GepgPaymentAck));

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
