using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.Modules.InsureeModule.Models;
using OpenImis.RestApi.Security;

namespace OpenImis.RestApi.Controllers
{
    [Authorize]
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

        [HasRights(Rights.PolicyAdd)]
        [HttpPost]
        [Route("Policies/Enter_Policy")]
        public virtual IActionResult Enter_Policy([FromBody]Policy model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            int userId = Convert.ToInt32(HttpContext.User.Claims
                .Where(w => w.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetPolicyLogic().Enter(model);

            return Json(response);
        }

        [HasRights(Rights.PolicyRenew)]
        [HttpPost]
        [Route("Policies/Renew_Policy")]
        public virtual IActionResult Renew_Policy([FromBody]Policy model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            int userId = Convert.ToInt32(HttpContext.User.Claims
                .Where(w => w.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            try
            {
                _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);
            }
            catch (Exception)
            {
                _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(-1);
            }

            var response = _imisModules.GetInsureeModule().GetPolicyLogic().Renew(model);

            return Json(response);
        }

        [HasRights(Rights.PolicySearch)]
        [HttpPost]
        [Route("Policies/Get_Commissions")]
        public virtual IActionResult Get_Commissions([FromBody]GetCommissionInputs model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            int userId = Convert.ToInt32(HttpContext.User.Claims
                .Where(w => w.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            try
            {
                _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);
            }
            catch (Exception)
            {
                _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(-1);
            }

            var response = _imisModules.GetInsureeModule().GetPolicyLogic().GetCommissions(model);

            return Json(response);
        }
    }
}