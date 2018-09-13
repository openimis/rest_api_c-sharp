using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Chanels.Sms;
using ImisRestApi.Data;
using ImisRestApi.Escape;
using ImisRestApi.Models;
using ImisRestApi.Repo;
using ImisRestApi.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImisRestApi.Controllers
{
    public class PaymentController : Controller
    {
        private ImisPayment _imisPayment;
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PaymentController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _imisPayment = new ImisPayment(_configuration, _hostingEnvironment);
        }
        //Recieve Payment from Operator/
        [HttpGet]
        [Route("api/Payment")]
        public IActionResult Index([FromBody]PaymentDetail payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }

        //Recieve Payment from Operator/
        [HttpPost]
        [Route("api/GetControlNumber")]
        public async Task<IActionResult> ControlNumber([FromBody]IntentOfPay intent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (intent.PaymentDetails == null) {
                PaymentDetail detail = new PaymentDetail();

                if(intent.InsureeNumber != null && intent.EnrolmentType != null && intent.ProductCode != null)
                {
                    detail.InsureeNumber = intent.InsureeNumber;
                    detail.ProductCode = intent.ProductCode;
                    detail.PaymentType = intent.EnrolmentType;
                   
                }
                else
                {
                    return BadRequest(ModelState);
                }
                List<PaymentDetail> details = new List<PaymentDetail>();
                details.Add(detail);

                intent.PaymentDetails = details;
            }
            //save the intent of pay 
           // _imisPayment.SaveIntent(intent);

            string url = _configuration["PaymentGateWay:Url"] + _configuration["PaymentGateWay:CNRequest"];

            ImisPayment payment = new ImisPayment(_configuration,_hostingEnvironment);
           // payment.GenerateCtrlNoRequest(intent.OfficerCode,intent.InsureeNumber, _imisPayment.PaymentId, _imisPayment.ExpectedAmount,intent.PaymentDetails);

            //ControlNumberRequest response = ControlNumberChanel.PostRequest(url, _paymentRepo.PaymentId, _paymentRepo.ExpectedAmount);

            //if (response.ControlNumber != null)
            //{
            //    _paymentRepo.SaveControlNumber(response.ControlNumber);

            //}
            //else if (response.ControlNumber == null)
            //{
            //    _paymentRepo.SaveControlNumber();
            //}
            //else if (response.RequestAcknowledged)
            //{
            //    _paymentRepo.SaveControlNumberAkn(response.RequestAcknowledged,"");
            //}

            string test = await Message.PushSMS("Your Request for control number was Sent", "+255767057265");

            return Json(new { status = true, sms_reply = true, sms_text = "Your Request for control number was Sent" });
            //return Ok("Request sent");
        }

        [HttpPost]
        [Route("api/GetControlNumberAck")]
        public IActionResult ControlNumberAck([FromBody]Acknowledgement model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //update Payment with the Id model.PaymentId flags to RequestPosted

            _imisPayment.SaveControlNumberAkn(true, "");

            return Ok("Control Number Acknowledgement Received");
        }

        [HttpPost]
        [Route("api/GetReqControlNumber")]
        public IActionResult ReceiveControlNumber([FromBody]GepgBillResponse model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach (var bill in model.BillTrxInf)
            {
                _imisPayment.PaymentId = bill.BillId;
                _imisPayment.SaveControlNumber(bill.PayCntrNum.ToString());

                //SendSMS

            }

            string resp = _imisPayment.ControlNumberResp();
            return Ok(resp);
        }

        [HttpPost]
        [Route("api/GetPaymentData")]
        public IActionResult GetPayment([FromBody]GepgPaymentMessage model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<PymtTrxInf> payments = model.PymtTrxInf;
            foreach(var payment in payments)
            {
                _imisPayment.SavePayment(payment);

            }

            string resp = _imisPayment.PaymentResp();
            return Ok(resp);
        }

        [HttpPost]
        [Route("api/GetReconciliationData")]
        public IActionResult GetReconciliation([FromBody]GepgReconcMessage model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resp = _imisPayment.ReconciliationResp();
            return Ok(resp);
        }
    }
}
