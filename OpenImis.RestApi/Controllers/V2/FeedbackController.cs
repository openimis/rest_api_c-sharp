using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV2;
using OpenImis.ModulesV2.FeedbackModule.Models;
using OpenImis.ModulesV2.Utils;
using OpenImis.RestApi.Security;

namespace OpenImis.RestApi.Controllers.V2
{
    [ApiVersion("2")]
    [Authorize]
    [Route("api/feedback/")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly IImisModules _imisModules;

        public FeedbackController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HasRights(Rights.ClaimFeedback)]
        [HttpPost]
        public IActionResult Post([FromBody]FeedbackRequest model)
        {
            int response;

            try
            {
                response = _imisModules.GetFeedbackModule().GetFeedbackLogic().Post(model);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            return Ok(response);
        }

        [HasRights(Rights.ClaimFeedback)]
        [HttpGet]
        public IActionResult Get()
        {
            List<FeedbackResponseModel> response;

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

                Repository rep = new Repository();
                string officerCode = rep.GetLoginNameByUserUUID(userUUID);

                response = _imisModules.GetFeedbackModule().GetFeedbackLogic().Get(officerCode);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            return Ok(response);
        }
    }
}