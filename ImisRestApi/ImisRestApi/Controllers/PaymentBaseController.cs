using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Chanels.Sms;
using ImisRestApi.Extensions;
using ImisRestApi.Logic;
using ImisRestApi.Models;
using ImisRestApi.Models.Payment;
using ImisRestApi.Models.Payment.Response;
using ImisRestApi.Models.Sms;
using ImisRestApi.Repo;
using ImisRestApi.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
 
        [Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/MatchPayment")]
        public virtual async Task<IActionResult> MatchPayment([FromBody]MatchModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error_occured = true, error_message = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage });

            try
            {
                var response = await _payment.MatchPayment(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new { error_occured = true, error_message = "Unknown Error Occured"});
            }
            
        }

        //Recieve Payment from Operator/
        [Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/GetControlNumber")]
        [ProducesResponseType(typeof(GetControlNumberResp), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetControlNumber([FromBody]IntentOfPay intent)
        {
            if (!ModelState.IsValid)
            {           
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;

                if(intent != null)
                {
                    var resp = await _payment.SaveIntent(intent, error.GetErrorNumber(), error.GetErrorMessage());
                }
                
                return BadRequest(new ErrorResponseV2(){ error_occured = true, error_message = error});
            }

            try
            {
                var response = await _payment.SaveIntent(intent);

                AssignedControlNumber data = (AssignedControlNumber)response.Data;
              
                return Ok(new GetControlNumberResp(){ error_occured = response.ErrorOccured, error_message = response.MessageValue,internal_identifier = data.internal_identifier, control_number = data.control_number });

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponseV2 (){ error_occured = true, error_message = e.Message });
            }
        }

        [Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/PostReqControlNumberAck")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual IActionResult PostReqControlNumberAck([FromBody]Acknowledgement model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new ErrorResponse(){ error_message = error });
            }

            try
            {
                var response = _payment.SaveAcknowledgement(model);
                if (response.Code == 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new ErrorResponse() { error_message = response.MessageValue });
                }
             
            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponse() { error_message = "Unknown Error Occured" });
            }
        }

        [Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/GetReqControlNumber")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual IActionResult GetReqControlNumber([FromBody]ControlNumberResp model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new ErrorResponse (){ error_message = error });
            }

            try
            {
                var response = _payment.SaveControlNumber(model);
                if (response.Code == 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new ErrorResponse() { error_message = response.MessageValue });
                }
               
            }
            catch (Exception)
            {
                return BadRequest(new { error_occured = true, error_message = "Unknown Error Occured" });
            }

        }

        [Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/GetPaymentData")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(PaymentDataBadResp), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual IActionResult GetPaymentData([FromBody]PaymentData model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new PaymentDataBadResp(){ error_message = error });
            }

            try
            {
                var response = _payment.SavePayment(model);

                if (response.Code == 0) {
                    return Ok();
                }
                else
                {
                    return BadRequest(new PaymentDataBadResp() { error_message = response.MessageValue });
                }

            }
            catch (Exception)
            {
                return BadRequest(new PaymentDataBadResp() { error_message = "Unknown Error Occured" });
            }

        }

        [Authorize(Roles = "PaymentSearch")]
        [HttpPost]
        [Route("api/GetAssignedControlNumbers")]
        [ProducesResponseType(typeof(AsignedControlNumbersResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetAssignedControlNumbers([FromBody]PaymentRequest requests)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new ErrorResponseV2(){ error_occured = true, error_message = error });
            }

            try
            {
                var response = await _payment.GetControlNumbers(requests);

                if (response.Code == 0)
                {
                    var controlNumbers = (List<AssignedControlNumber>)response.Data;
                    return Ok(new AsignedControlNumbersResponse() { error_occured = response.ErrorOccured, assigned_control_numbers = controlNumbers });
                }
                else
                {
                    return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = response.MessageValue });
                }
                

            }
            catch (Exception)
            {
                return BadRequest(new ErrorResponseV2(){ error_occured = true, error_message = "Unknown Error Occured" });
            }
        }
    }
}
