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

namespace ImisRestApi.Controllers
{
    public class PaymentController : PaymentBaseController
    {
        private ImisPayment imisPayment;
        public PaymentController(IConfiguration configuration, IHostingEnvironment hostingEnvironment) :base(configuration, hostingEnvironment)
        {
            imisPayment = new ImisPayment(configuration, hostingEnvironment);
        }

        [HttpPost]
        [Route("api/GetControlNumber/Single")]
        public async Task<IActionResult> Index([FromBody]IntentOfSinglePay payment)
        {
            payment.PhoneNumber = payment.Msisdn;
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { success = false, error_occured = true, error_message = error });
            }

            try
            {
                payment.SetDetails();

                var response = await _payment.SaveIntent(payment);

                return Ok(new { success = !response.ErrorOccured, error_occured = response.ErrorOccured, error_message = response.MessageValue, control_number = response.Data });

            }
            catch (Exception e)
            {
                return Ok(new { success = false, error_occured = true, error_message = e.Message });
            }
        }

        [HttpPost]
        [Route("api/GetReconciliationData")]
        public IActionResult GetReconciliation([FromBody]GepgReconcMessage model)
        {

            if (imisPayment.IsValidCall(model,2))
            {
                if (!ModelState.IsValid)
                    return BadRequest(imisPayment.ReconciliationResp(7242));

                return Ok(imisPayment.ReconciliationResp(7101));
            }
            else
            {
                return BadRequest(imisPayment.ReconciliationResp(7303));
            }
        }

        [NonAction]
        public override IActionResult GetReqControlNumber([FromBody] ControlNumberResp model)
        {
            return base.GetReqControlNumber(model);
        }

        [NonAction]
        public override IActionResult GetPaymentData([FromBody] PaymentData model)
        {
            return base.GetPaymentData(model);
        }

        [NonAction]
        public override IActionResult PostReqControlNumberAck([FromBody] Acknowledgement model)
        {
            return base.PostReqControlNumberAck(model);
        }

        [HttpPost]
        [Route("api/GetPaymentData")]
        public IActionResult GetPaymentChf([FromBody]GepgPaymentMessage model)
        {
           
            if (imisPayment.IsValidCall(model, 1))
            {
                if (!ModelState.IsValid)
                    return BadRequest(imisPayment.PaymentResp(7242));

                List<PymtTrxInf> payments = model.PymtTrxInf;
                foreach (var payment in payments)
                {
                    PaymentData pay = new PaymentData()
                    {
                        PaymentId = payment.BillId,
                        ControlNumber = payment.PayCtrNum,
                        ProductCode = payment.PayRefId,
                        EnrolmentOfficerCode = payment.PyrName,
                        TransactionId = payment.TrxId,
                        ReceivedAmount = Convert.ToDouble(payment.BillAmt),
                        ReceivedDate = DateTime.UtcNow,
                        PaymentDate = Convert.ToDateTime(payment.TrxDtTm),
                        PaymentOrigin = "GePG",
                        ReceiptNumber = payment.PspReceiptNumber,
                        PhoneNumber = payment.PyrCellNum.ToString()
                    };

                    base.GetPaymentData(pay);

                }

                return Ok(imisPayment.PaymentResp(7101));
            }
            else
            {
                return BadRequest(imisPayment.PaymentResp(7303));
            }
            
        }

        [HttpPost]
        [Route("api/GetReqControlNumber")]
        public IActionResult GetReqControlNumberChf([FromBody] GepgBillResponse model)
         {
            
            if (imisPayment.IsValidCall(model, 0))
            {
                if (!ModelState.IsValid)
                    return BadRequest(imisPayment.ControlNumberResp(7242));

                foreach (var bill in model.BillTrxInf)
                {
                    ControlNumberResp ControlNumberResponse = new ControlNumberResp()
                    {
                        PaymentId = bill.BillId,
                        ControlNumber = bill.PayCntrNum,
                        ErrorOccured = false,
                        ErrorMessage = bill.TrxStsCode
                    };

                    try
                    {
                        var response = base.GetReqControlNumber(ControlNumberResponse);
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }

                return Ok(imisPayment.ControlNumberResp(7101));
            }
            else
            {
                return BadRequest(imisPayment.ControlNumberResp(7303));
            }

        }

    }
}