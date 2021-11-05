using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV2;
using OpenImis.ModulesV2.PolicyModule.Models;
using OpenImis.ModulesV2.Utils;
using OpenImis.Security.Security;

namespace OpenImis.RestApi.Controllers.V2
{
    [ApiVersion("2")]
    [Authorize]
    [Route("api/policy/")]
    [ApiController]
    public class PolicyRenewalController : Controller
    {
        private readonly IImisModules _imisModules;

        public PolicyRenewalController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HasRights(Rights.PolicySearch)]
        [HttpGet]
        public IActionResult Get()
        {
            List<GetPolicyRenewalModel> response;

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

                Repository rep = new Repository();
                string officerCode = rep.GetLoginNameByUserUUID(userUUID);

                response = _imisModules.GetPolicyModule().GetPolicyRenewalLogic().Get(officerCode);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            return Ok(response);
        }

        [HasRights(Rights.PolicyRenew)]
        [HttpPost]
        [Route("renew")]
        public IActionResult Post([FromBody]PolicyRenewalModel model)
        {
            int response;

            try
            {
                response = _imisModules.GetPolicyModule().GetPolicyRenewalLogic().Post(model);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            return Ok(response);
        }

        [HasRights(Rights.PolicyDelete)]
        [HttpDelete]
        [Route("renew/{uuid}")]
        public IActionResult Delete(Guid uuid)
        {
            int response;

            try
            {
                response = _imisModules.GetPolicyModule().GetPolicyRenewalLogic().Delete(uuid);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            return Ok(response);
        }

        [HasRights(Rights.PolicySearch)]
        [HttpPost]
        [Route("commissions")]
        public virtual IActionResult GetCommissions([FromBody]GetCommissionInputs model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            var response = _imisModules.GetPolicyModule().GetPolicyRenewalLogic().GetCommissions(model);

            return Json(response);
        }
    }
}