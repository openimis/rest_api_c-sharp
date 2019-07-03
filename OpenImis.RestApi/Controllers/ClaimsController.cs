using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.Modules.ClaimModule.Models;

namespace OpenImis.RestApi.Controllers
{
    //[ApiVersion("1")]
    //[Authorize(Roles = "IMISAdmin, EnrollmentOfficer")]
    [Route("api/")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class ClaimsController : Controller
    {
        private readonly IImisModules _imisModules;
        public ClaimsController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

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

        //[Authorize(Roles = "ClaimAdd")]
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