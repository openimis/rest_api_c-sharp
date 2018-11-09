using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Chanels;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Data;
using ImisRestApi.Models;
using ImisRestApi.Models.Payment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ImisRestApi.Controllers
{
    public class PaymentController : PaymentBaseController
    {
        private ImisPayment p;
        public string ReconciliationFolder;

        public PaymentController(IConfiguration configuration, IHostingEnvironment hostingEnvironment) :base(configuration, hostingEnvironment)
        {
            ReconciliationFolder = hostingEnvironment.ContentRootPath + @"\Chanels\Payment\Reconciliation";
            p = new ImisPayment(configuration, hostingEnvironment);
        }

        [HttpPost]
        [Route("api/GetControlNumber/Single")]
        public async Task<IActionResult> Index([FromBody]IntentOfSinglePay payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            payment.SetDetails();

            var response = await _payment.SaveIntent(payment);

            return Ok(new { error_occured = response.ErrorOccured, error_message = response.MessageValue, control_number = response.Data });
        }

        [HttpPost]
        [Route("api/GetReconciliationData")]
        public IActionResult GetReconciliation([FromBody]GepgReconcMessage model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //System.IO.File.WriteAllText(ReconciliationFolder, JsonConvert.SerializeObject(model));
            return Ok(p.ReconciliationResp());
        }

        [NonAction]
        public override IActionResult ReceiveControlNumber([FromBody] ControlNumberResp model)
        {
            return base.ReceiveControlNumber(model);
        }

        [NonAction]
        public override IActionResult GetPayment([FromBody] PaymentData model)
        {
            return base.GetPayment(model);
        }

        [NonAction]
        public override IActionResult ControlNumberAck([FromBody] Acknowledgement model)
        {
            return base.ControlNumberAck(model);
        }

        [HttpPost]
        [Route("api/GetPaymentData")]
        public IActionResult GetPaymentChf([FromBody]GepgPaymentMessage model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<PymtTrxInf> payments = model.PymtTrxInf;
            foreach (var payment in payments)
            {
                PaymentData pay = new PaymentData() {
                    PaymentId = payment.BillId,
                    ControlNumber = payment.PayCtrNum.ToString(),
                    ProductCode = payment.PayRefId,
                    EnrolmentOfficerCode = payment.PyrName,
                    TransactionId = payment.TrxId,
                    ReceivedAmount = Convert.ToDouble(payment.BillAmt),
                    ReceivedDate = payment.PaymentDate,
                    PaymentDate = payment.PaymentDate,
                    PaymentOrigin = payment.PaymentOrigin,
                    ReceiptNumber = payment.PspReceiptNumber,
                    PhoneNumber = payment.PyrCellNum.ToString()
                };

                base.GetPayment(pay);

            }

            return Ok(p.PaymentResp());
        }

        [HttpPost]
        [Route("api/GetReqControlNumber")]
        public IActionResult ReceiveControlNumberChf([FromBody] GepgBillResponse model)
         {
            foreach (var bill in model.BillTrxRespInf)
            {
                ControlNumberResp ControlNumberResponse = new ControlNumberResp()
                {
                    PaymentId = bill.BillId,
                    ControlNumber = bill.PayCntrNum.ToString(),
                    ErrorOccured = false,
                    ErrorMessage = bill.TrxStsCode
                };

                try
                {
                    var response = base.ReceiveControlNumber(ControlNumberResponse);
                }
                catch (Exception e)
                {

                    throw e;
                }
            }

            return Ok(p.ControlNumberResp());
        }
    }
}