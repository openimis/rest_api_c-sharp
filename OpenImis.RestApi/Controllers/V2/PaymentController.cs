using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV2;
using OpenImis.ModulesV2.PaymentModule.Helpers.Extensions;
using OpenImis.ModulesV2.PaymentModule.Models;
using OpenImis.ModulesV2.PaymentModule.Models.Response;
using OpenImis.RestApi.Security;

namespace OpenImis.RestApi.Controllers.V2
{
    [ApiVersion("2")]
    [Authorize]
    [Route("api/payment/")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IImisModules _imisModules;
        public readonly IHostingEnvironment _hostingEnvironment;

        public PaymentController(IImisModules imisModules, IHostingEnvironment hostingEnvironment)
        {
            _imisModules = imisModules;
            _hostingEnvironment = hostingEnvironment;

            _imisModules.GetPaymentModule().GetPaymentLogic().WebRootPath = _hostingEnvironment.WebRootPath;
            _imisModules.GetPaymentModule().GetPaymentLogic().ContentRootPath = _hostingEnvironment.ContentRootPath;
        }

        [HasRights(Rights.PaymentAdd)]
        [HttpPost]
        [Route("GetControlNumber")]
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