using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.ClaimModule.Models;
using OpenImis.ModulesV3.ClaimModule.Models.RegisterClaim;
using OpenImis.Security.Security;

namespace OpenImis.RestApi.Controllers.V3
{
    [ApiVersion("3")]
    [Authorize]
    [Route("api/")]
    [ApiController]
    public class ClaimController : Controller
    {
        private readonly IImisModules _imisModules;

        public ClaimController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HasRights(Rights.ClaimAdd)]
        [Route("claim")]
        [HttpPost]
        public IActionResult Create([FromBody]Claim claim)
        {
            int response;

            try
            {
                response = _imisModules.GetClaimModule().GetClaimLogic().Create(claim);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            return Ok(response);
        }

        //[HasRights(Rights.DiagnosesDownload)]
        [AllowAnonymous]
        [HttpPost]
        [Route("GetDiagnosesServicesItems")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult GetDiagnosesServicesItems([FromBody]DsiInputModel model)
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
                return BadRequest(new { error_occured = true, error_message = e.Message });
            }
        }

        [HasRights(Rights.ClaimAdd)]
        [HttpPost]
        [Route("getpaymentlists")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult GetPaymentLists([FromBody]PaymentListsInputModel model)
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
                return BadRequest(new { error_occured = true, error_message = e.Message });
            }
        }

        //[HasRights(Rights.FindClaimAdministrator)]
        [AllowAnonymous]
        [HttpGet]
        [Route("claims/getclaimadmins")]
        public IActionResult ValidateClaimAdmin()
        {
            try
            {
                var data = _imisModules.GetClaimModule().GetClaimLogic().GetClaimAdministrators();
                return Ok(new { error_occured = false, claim_admins = data });
            }
            catch (Exception e)
            {
                return BadRequest(new { error_occured = true, error_message = e.Message });
            }
        }

        //[HasRights(Rights.ClaimSearch)]
        [AllowAnonymous]
        [HttpGet]
        [Route("claims/controls")]
        public IActionResult GetControls()
        {
            try
            {
                var data = _imisModules.GetClaimModule().GetClaimLogic().GetControls();
                return Ok(new { error_occured = false, controls = data });
            }
            catch (Exception e)
            {
                return BadRequest(new { error_occured = true, error_message = e.Message });
            }
        }
    }
}