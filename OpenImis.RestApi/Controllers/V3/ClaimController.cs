using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV3.ClaimModule.Models;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.ClaimModule.Models.RegisterClaim;
using Microsoft.Extensions.Logging;
using OpenImis.RestApi.Util.ErrorHandling;
using System.Net;

namespace OpenImis.RestApi.Controllers.V3
{
    [Produces("application/json")]
    [ApiVersion("3")]
    [Authorize]
    [Route("api/claim/")]
    [ApiController]
    public class ClaimController : Controller
    {
        private IImisModules _imisModules;
        private readonly ILogger _logger;

        public ClaimController(IImisModules imisModules, ILoggerFactory loggerFactory)
        {
            _imisModules = imisModules;
            _logger = loggerFactory.CreateLogger<ClaimController>();
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult Create([FromBody] Claim claim)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            try
            {
                var response = _imisModules.GetClaimModule().GetClaimLogic().Create(claim);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new SubmitClaimResponse
                {
                    ClaimCode = claim.Details.ClaimCode,
                    Response = (int)Errors.Claim.UnexpectedException,
                    Message = e.Message
                });
            }
        }

        [HttpPost]
        [Route("GetDiagnosesServicesItems")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult GetDiagnosesServicesItems([FromBody] DsiInputModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            try
            {
                var response = _imisModules.GetClaimModule().GetClaimLogic().GetDsi(model);
                return Json(response);
            }
            catch (Exception e)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost]
        [Route("GetPaymentLists")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult GetPaymentLists([FromBody] PaymentListsInputModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            try
            {
                var response = _imisModules.GetClaimModule().GetClaimLogic().GetPaymentLists(model);

                return Json(response);
            }
            catch (Exception e)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpGet]
        [Route("GetClaimAdmins")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult ValidateClaimAdmin()
        {
            try
            {
                var data = _imisModules.GetClaimModule().GetClaimLogic().GetClaimAdministrators();
                return Ok(new { error_occured = false, claim_admins = data });
            }
            catch (Exception e)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, e.Message);
            }

        }

        [HttpGet]
        [Route("Controls")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult GetControls()
        {
            try
            {
                var data = _imisModules.GetClaimModule().GetClaimLogic().GetControls();
                return Ok(new { error_occured = false, controls = data });
            }
            catch (Exception e)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost]
        [Route("GetClaims")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult GetClaims([FromBody] ClaimsModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            try
            {
                var data = _imisModules.GetClaimModule().GetClaimLogic().GetClaims(model);

                return Ok(new { error_occured = false, data = data });
            }
            catch (Exception e)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}