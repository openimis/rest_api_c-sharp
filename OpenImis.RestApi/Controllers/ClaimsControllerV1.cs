using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV1;
using OpenImis.ModulesV1.ClaimModule.Models;
using OpenImis.RestApi.Security;

namespace OpenImis.RestApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/")]
    [ApiController]
    public class ClaimsControllerV1 : Controller
    {
        private readonly IImisModules _imisModules;
        public ClaimsControllerV1(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HasRights(Rights.DiagnosesDownload)]
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
        [Route("GetPaymentLists")]
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

        [HasRights(Rights.FindClaimAdministrator)]
        [HttpGet]
        [Route("Claims/GetClaimAdmins")]
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

        [HasRights(Rights.ClaimSearch)]
        [HttpGet]
        [Route("Claims/Controls")]
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