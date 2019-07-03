using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.Modules.InsureeModule.Models;

namespace OpenImis.RestApi.Controllers
{
    [Route("api/")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class PolicyController : Controller
    {
        private readonly IImisModules _imisModules;
        public PolicyController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HttpPost]
        [Route("Policies/Enter_Policy")]
        public virtual IActionResult Enter_Policy([FromBody]Policy model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //_imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(Convert.ToInt32(identity.FindFirst("UserId").Value));

            // Temporary
            var userId = 1;
            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetPolicyLogic().Enter(model);

            return Json(response);
        }

        [HttpPost]
        [Route("Policies/Renew_Policy")]
        public virtual IActionResult Renew_Policy([FromBody]Policy model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var iden = identity.FindFirst("UserId");

            //try
            //{
            //    policies.UserId = Convert.ToInt32(iden.Value);
            //}
            //catch (Exception e)
            //{
            //    policies.UserId = -1;
            //}

            // Temporary
            var userId = 1;
            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetPolicyLogic().Renew(model);

            return Json(response);
        }

        [HttpPost]
        [Route("Policies/Get_Commissions")]
        public virtual IActionResult Get_Commissions([FromBody]GetCommissionInputs model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var iden = identity.FindFirst("UserId");

            //try
            //{
            //    policies.UserId = Convert.ToInt32(iden.Value);
            //}
            //catch (Exception e)
            //{
            //    policies.UserId = -1;
            //}

            // Temporary
            var userId = 1;
            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetPolicyLogic().GetCommissions(model);

            return Json(response);
        }
    }
}