using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV2;
using OpenImis.ModulesV2.InsureeModule.Models;
using OpenImis.RestApi.Security;

namespace OpenImis.RestApi.Controllers.V2
{
    [ApiVersion("2")]
    [Authorize]
    [Route("api/policy/")]
    [ApiController]
    public class PolicyController : Controller
    {
        private readonly IImisModules _imisModules;

        public PolicyController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HasRights(Rights.PolicySearch)]
        [HttpGet]
        public IActionResult Get()
        {
            List<GetPolicyModel> response;

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

                string officerCode = _imisModules.GetInsureeModule().GetPolicyLogic().GetLoginNameByUserUUID(userUUID);

                response = _imisModules.GetInsureeModule().GetPolicyLogic().Get(officerCode);
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
                response = _imisModules.GetInsureeModule().GetPolicyLogic().Post(model);
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
                response = _imisModules.GetInsureeModule().GetPolicyLogic().Delete(uuid);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            return Ok(response);
        }
    }
}