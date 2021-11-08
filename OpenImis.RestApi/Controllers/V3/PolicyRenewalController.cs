﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.PolicyModule.Models;
using OpenImis.ModulesV3.Utils;
using OpenImis.RestApi.Util.ErrorHandling;
using OpenImis.Security.Security;

namespace OpenImis.RestApi.Controllers.V3
{
    [ApiVersion("3")]
    [Authorize]
    [Route("api/policy/")]
    [ApiController]
    public class PolicyRenewalController : Controller
    {
        private readonly IImisModules _imisModules;
        private readonly ILogger _logger;

        public PolicyRenewalController(IImisModules imisModules, ILoggerFactory loggerFactory)
        {
            _imisModules = imisModules;
            _logger = loggerFactory.CreateLogger<PolicyRenewalController>();
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
                throw new BusinessException(e.Message);
            }

            return Ok(response);
        }

        [HasRights(Rights.PolicyRenew)]
        [HttpPost]
        [Route("renew")]
        public IActionResult Post([FromBody] PolicyRenewalModel model)
        {
            int response;

            try
            {
                response = _imisModules.GetPolicyModule().GetPolicyRenewalLogic().Post(model);
            }
            catch (ValidationException e)
            {
                throw new BusinessException(e.Message);
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
                throw new BusinessException(e.Message);
            }

            return Ok(response);
        }

        [HasRights(Rights.PolicySearch)]
        [HttpPost]
        [Route("commissions")]
        public virtual IActionResult GetCommissions([FromBody] GetCommissionInputs model)
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