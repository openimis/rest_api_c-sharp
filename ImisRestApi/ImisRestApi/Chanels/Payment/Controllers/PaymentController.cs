using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Chanels.Payment.Escape;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Chanels.Payment.Response;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImisRestApi.Chanels.Payment.Controllers
{
    public class PaymentController : Controller
    {

        //Recieve Payment from Operator/
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

            if (intent.Renewal)
            {
                //save the intent of pay 

                foreach(var i in intent.PaymentDetails)
                {
                    //if i.InsureeNumber Exists

                    //Else GetLocalDefaults
                    var memberCount = LocalDefault.FamilyMambers(); //get amount to be paid by these members (i.ExpectedAmount)
                     
                    //Save i to database and get the internal identifier (i.Id)
                }

                ControlNumberRequest response = ControlNumberChanel.PostRequest();

                if(response.ControlNumber != null)
                {
                    //Save THe control number 
                    //SendSMS
                }
                else if (response.RequestAcknowledged)
                {
                    //Update Payment RewuestPosted = true
                }
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
