using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.FeedbackModule.Models;
using OpenImis.ModulesV3.Utils;
using OpenImis.RestApi.Util.ErrorHandling;
using OpenImis.Security.Security;

namespace OpenImis.RestApi.Controllers.V3
{
    [ApiVersion("3")]
    [Authorize]
    [Route("api/feedback/")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly IImisModules _imisModules;
        private readonly ILogger _logger;

        public FeedbackController(IImisModules imisModules, ILoggerFactory loggerFactory)
        {
            _imisModules = imisModules;
            _logger = loggerFactory.CreateLogger<FeedbackController>();
        }

        [HasRights(Rights.ClaimFeedback)]
        [HttpPost]
        public IActionResult Post([FromBody] FeedbackRequest model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            int response;
            try
            {
                response = _imisModules.GetFeedbackModule().GetFeedbackLogic().Post(model);
            }
            catch (ValidationException e)
            {
                throw new BusinessException(e.Message);
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
                throw new BusinessException(e.Message);
            }

            return Ok(response);
        }
    }
}