using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OpenImis.ePayment.Escape.Payment.Models;
using OpenImis.ePayment.Escape.Sms;
using OpenImis.ePayment.Extensions;
using OpenImis.ePayment.Logic;
using OpenImis.ePayment.Models;
using OpenImis.ePayment.Models.Payment;
using OpenImis.ePayment.Models.Payment.Response;
using OpenImis.ePayment.Models.Sms;
using OpenImis.ePayment.Repo;
using OpenImis.ePayment.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenImis.ePayment.Responses;

namespace OpenImis.ePayment.Controllers
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



        //[Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/MatchPayment")]
        public virtual async Task<IActionResult> MatchPayment([FromBody] MatchModel model)
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
                return BadRequest(new { error_occured = true, error_message = "Unknown Error Occured" });
            }

        }

        //Recieve Payment from Operator/
        //[Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/GetControlNumber")]
        [ProducesResponseType(typeof(GetControlNumberResp), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetControlNumber([FromBody] IntentOfPay intent)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;

                if (intent != null)
                {
                    // var resp = await _payment.SaveIntent(intent, error.GetErrorNumber(), error.GetErrorMessage());
                    return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = error });
                }
                else
                {
                    return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = "10-Uknown type of payment" });
                }
            }

            try
            {
                var response = await _payment.SaveIntent(intent);


                AssignedControlNumber data = (AssignedControlNumber)response.Data;

                return Ok(new GetControlNumberResp() { error_occured = response.ErrorOccured, error_message = response.MessageValue, internal_identifier = data.internal_identifier, control_number = data.control_number });

            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = e.Message });
            }
        }

        [HttpPost]
        [Route("api/RequestBulkControlNumbers")]
        [ProducesResponseType(typeof(GetControlNumberResp), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> RequestBulkControlNumbers([FromBody] RequestBulkControlNumbersModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;

                if (model != null)
                {
                    return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = error });
                }
                else
                {
                    return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = "10-Uknown type of payment" });
                }
            }

            // var officer = _payment.GetOfficerInfo(model.OfficerId);

            var result = await _payment.RequestBulkControlNumbers(model);

            return Ok(result);
        }

        //[Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/PostReqControlNumberAck")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> PostReqControlNumberAck([FromBody] Acknowledgement model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new ErrorResponse() { error_message = error });
            }

            try
            {
                var response = await _payment.SaveAcknowledgementAsync(model);
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

        //[Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/GetReqControlNumber")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetReqControlNumber([FromBody] ControlNumberResp model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new ErrorResponse() { error_message = error });
            }

            try
            {
                var response = await _payment.SaveControlNumberAsync(model);
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

        [HttpPost]
        [Route("api/payment/cancel")]
        [ProducesResponseType(typeof(DataMessage), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> CancelPayment([FromBody] PaymentCancelBaseModel model)
        {

            try
            {
                return Ok(await _payment.CancelPayment(model.payment_id));
            }
            catch (Exception e)
            {
                return BadRequest(new PaymentDataBadResp() { error_message = e.ToString() });
            }
        }

        //[Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/GetPaymentData")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(PaymentDataBadResp), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetPaymentData([FromBody] PaymentData model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new PaymentDataBadResp() { error_message = error });
            }

            try
            {
                var response = await _payment.SavePayment(model);

                if (response.Code == 0)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new PaymentDataBadResp() { error_message = response.MessageValue });
                }

            }
            catch (Exception e)
            {
                return BadRequest(new PaymentDataBadResp() { error_message = "Unknown Error Occured" });
            }

        }

        //[Authorize(Roles = "PaymentSearch")]
        [HttpPost]
        [Route("api/GetAssignedControlNumbers")]
        [ProducesResponseType(typeof(AsignedControlNumbersResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetAssignedControlNumbers([FromBody] PaymentRequest requests)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = error });
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
                return BadRequest(new ErrorResponseV2() { error_occured = true, error_message = "Unknown Error Occured" });
            }
        }

        //[Authorize(Roles = "PaymentAdd")]
        [HttpPost]
        [Route("api/ProvideReconciliationData")]
        public virtual async Task<IActionResult> ProvideReconciliationData([FromBody] ReconciliationRequest model)
        {
            ValidationResult validation = new ValidationBase().ReconciliationData(model);

            if (validation != ValidationResult.Success)
            {
                return BadRequest(new { error_occured = true, error_message = validation.ErrorMessage });
            }

            try
            {
                var response = await _payment.ProvideReconciliationData(model);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new { error_occured = true, error_message = "Unknown Error Occured" });
            }


        }

        [HttpPost]
        [Route("api/GetControlNumbersForEO")]
        [ProducesResponseType(typeof(BulkControlNumbersForEO), 200)]
        [ProducesResponseType(typeof(ErrorResponseV2), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public virtual async Task<IActionResult> GetControlNumbersForEO([FromBody] GetControlNumbersForEOModel model)
        {
            BulkControlNumbersForEO response = null;
            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());
                var officerId = new ValidationBase().GetOfficerIdByUserUUID(userUUID, _configuration);
                var officerDetails = _payment.GetOfficerInfo(officerId);

                response = _payment.GetControlNumbersForEO(officerDetails.Code, model.ProductCode, model.AvailableControlNumbers);

                // Check if the product requested has enough CNs left
                try
                {
                    var count = await _payment.ControlNumbersToBeRequested(model.ProductCode);
                    if (count > 0)
                    {
                        // The following method is Async, but we do not await it since we don't want to wait for the result
                        _ = RequestBulkControlNumbers(new RequestBulkControlNumbersModel { ControlNumberCount = count, ProductCode = model.ProductCode });
                    }
                }
                catch (Exception)
                {


                }


            }
            catch (Exception ex)
            {

                return BadRequest(new { error_occured = true, error_message = "Unknown Error Occured" });
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("api/GetControlNumber/Single")]
        public virtual async Task<IActionResult> CHFRequestControlNumberForSimplePolicy([FromBody] IntentOfSinglePay intent)
        {
            return null;
        }
    }
}
