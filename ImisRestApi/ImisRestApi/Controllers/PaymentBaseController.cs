using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Chanels.Sms;

using ImisRestApi.Escape;
using ImisRestApi.Logic;
using ImisRestApi.Models;
using ImisRestApi.Models.Payment;
using ImisRestApi.Repo;
using ImisRestApi.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImisRestApi.Controllers
{
    public abstract class PaymentBaseController : Controller
    {
        public PaymentLogic _payment;
        public IConfiguration _configuration;
        public readonly IHostingEnvironment _hostingEnvironment;

        public PaymentBaseController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _payment = new PaymentLogic(configuration, hostingEnvironment);
        }
       
        //Recieve Payment from Operator/
        [HttpPost]
        [Route("api/GetControlNumber")]
        public virtual async Task<IActionResult> ControlNumber([FromBody]IntentOfPay intent)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error_occured = false, error_message = ModelState.FirstOrDefault().Value, control_number = "" });

            var response = await _payment.SaveIntent(intent);

            return Ok(new { error_occured = response.ErrorOccured,error_message = response.MessageValue,control_number = response.Data});
        }

        [HttpPost]
        [Route("api/GetControlNumberAck")]
        public virtual IActionResult ControlNumberAck([FromBody]Acknowledgement model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = _payment.SaveAcknowledgement(model);

            return Ok(new { error_occured = response.ErrorOccured, error_message = response.Data, control_number = response.Data });
        }

        [HttpPost]
        [Route("api/GetReqControlNumber")]
        public virtual IActionResult ReceiveControlNumber([FromBody]ControlNumberResp model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = _payment.SaveControlNumber(model);
            return Ok(new { error_occured = response.ErrorOccured, error_message = response.Data });
        }

        [HttpPost]
        [Route("api/GetPaymentData")]
        public virtual IActionResult GetPayment([FromBody]PaymentData model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = _payment.SavePayment(model);
            return Ok(new { error_occured = response.ErrorOccured, error_message = response.Data});
        }

      
    }
}
