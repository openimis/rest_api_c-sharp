using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenImis.Modules;
using OpenImis.Modules.PaymentModule.Helpers.Extensions;
using OpenImis.Modules.PaymentModule.Models;
using OpenImis.Modules.PaymentModule.Models.Response;
using OpenImis.RestApi.Security;
using Rights = OpenImis.RestApi.Security.Rights;

namespace OpenImis.RestApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class PaymentControllerV1 : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IImisModules _imisModules;
        public readonly IHostingEnvironment _hostingEnvironment;

        public PaymentControllerV1(IConfiguration configuration, IHostingEnvironment hostingEnvironment, IImisModules imisModules)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _imisModules = imisModules;

            _imisModules.GetPaymentModule().GetPaymentLogic().WebRootPath = _hostingEnvironment.WebRootPath;
            _imisModules.GetPaymentModule().GetPaymentLogic().ContentRootPath = _hostingEnvironment.ContentRootPath;
        }

        [HasRights(Rights.PaymentAdd)]
        [HttpPost]
        [Route("MatchPayment")]
        public virtual IActionResult MatchPayment([FromBody]MatchModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error_occured = true, error_message = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage });

            try
            {
                var response = _imisModules.GetPaymentModule().GetPaymentLogic().MatchPayment(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new { error_occured = true, error_message = "Unknown Error Occured" });
            }
        }

        //Recieve Payment from Operator/
        [HasRights(Rights.PaymentAdd)]
        [HttpPost]
        [Route("GetControlNumber")]
        //[ProducesResponseType(typeof(GetControlNumberResp), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetControlNumber([FromBody]IntentOfPay intent)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;

                if (intent != null)
                {
                    var resp = await _imisModules.GetPaymentModule().GetPaymentLogic().SaveIntent(intent, error.GetErrorNumber(), error.GetErrorMessage());
                    return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = error });
                }
                else
                {
                    return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = "10-Uknown type of payment" });
                }

            }

            try
            {
                var response = await _imisModules.GetPaymentModule().GetPaymentLogic().SaveIntent(intent);

                AssignedControlNumber data = (AssignedControlNumber)response.Data;

                return Ok(new GetControlNumberResp() { error_occured = response.ErrorOccured, error_message = response.MessageValue, internal_identifier = data.internal_identifier, control_number = data.control_number });

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = e.Message });
            }
        }

        [HasRights(Rights.PaymentAdd)]
        [HttpPost]
        [Route("PostReqControlNumberAck")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual IActionResult PostReqControlNumberAck([FromBody]Acknowledgement model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new ErrorResponse() { error_message = error });
            }

            try
            {
                var response = _imisModules.GetPaymentModule().GetPaymentLogic().SaveAcknowledgement(model);
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

        [HasRights(Rights.PaymentAdd)]
        [HttpPost]
        [Route("GetReqControlNumber")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual IActionResult GetReqControlNumber([FromBody]ControlNumberResp model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new ErrorResponse() { error_message = error });
            }

            try
            {
                var response = _imisModules.GetPaymentModule().GetPaymentLogic().SaveControlNumber(model);
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

        [HasRights(Rights.PaymentAdd)]
        [HttpPost]
        [Route("GetPaymentData")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(PaymentDataBadResp), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetPaymentData([FromBody]PaymentData model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new PaymentDataBadResp() { error_message = error });
            }

            try
            {
                var response = await _imisModules.GetPaymentModule().GetPaymentLogic().SavePayment(model);

                if (response.Code == 0)
                {
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

        [HasRights(Rights.PaymentSearch)]
        [HttpPost]
        [Route("GetAssignedControlNumbers")]
        [ProducesResponseType(typeof(AsignedControlNumbersResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetAssignedControlNumbers([FromBody]PaymentRequest requests)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = error });
            }

            try
            {
                var response = await _imisModules.GetPaymentModule().GetPaymentLogic().GetControlNumbers(requests);

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
                return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = "Unknown Error Occured" });
            }
        }
    }
}