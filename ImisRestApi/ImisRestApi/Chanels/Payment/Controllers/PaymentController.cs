using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Escape;
using ImisRestApi.Models;
using ImisRestApi.Repo;
using ImisRestApi.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImisRestApi.Controllers
{
    public class PaymentController : Controller
    {
        private PaymentRepo _paymentRepo;
        private IConfiguration _configuration;

        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
            
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
        public IActionResult ControlNumber([FromBody]IntentOfPay intent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //save the intent of pay 
            _paymentRepo = new PaymentRepo(_configuration, intent);

            _paymentRepo.SaveIntent();

            string url = _configuration["PaymentGateWay:Url"] + _configuration["PaymentGateWay:CNRequest"];

            ControlNumberRequest response = ControlNumberChanel.PostRequest(url, _paymentRepo.PaymentId, _paymentRepo.ExpectedAmount);

            if (response.ControlNumber != null)
            {
                _paymentRepo.SaveControlNumber(response.ControlNumber);

            }
            else if (response.ControlNumber == null)
            {
                _paymentRepo.SaveControlNumber();
            }
            else if (response.RequestAcknowledged)
            {
                _paymentRepo.SaveControlNumberAkn(response.RequestAcknowledged,"");
            }

            return Ok("Request sent");
        }

        [HttpPost]
        [Route("api/GetControlNumberAck")]
        public IActionResult ControlNumberAck([FromBody]Acknowledgement model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
  
            //update Payment with the Id model.PaymentId flags to RequestPosted

            return Ok("Control Number Acknowledgement Received");
        }

        [HttpPost]
        [Route("api/GetReqControlNumber")]
        public IActionResult ReceiveControlNumber([FromBody]ControlNumberContainer model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //update Control number table, PaymentId = model.PaymentId
            //SendSMS

            ControlNumberChanel.SendAcknowledgement();

            return Ok("Control Number Received");
        }

        [HttpPost]
        [Route("api/GetPaymentData")]
        public IActionResult GetPayment([FromBody]PaymentContainer model)
        {
            //Add to payment and payment details tables
            return Ok("Payment Received");
        }
    }
}
