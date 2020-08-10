using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Chanels.Sms;
using ImisRestApi.Controllers;
using ImisRestApi.Escape.Models;
using ImisRestApi.Models;
using ImisRestApi.Models.Sms;
using ImisRestApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Controllers
{
    public class ValidateController : ValidateBaseController
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ValidateController(IConfiguration configuration, IHostingEnvironment hostingEnvironment) :base(configuration)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [NonAction]
        public override IActionResult Validate_Credentials([FromBody] UserLogin userlogin)
        {
            return base.Validate_Credentials(userlogin);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Validate/Credentials")]
        public IActionResult Validate_ChfCredentials([FromBody] ChfUserLogin userlogin)
        {

            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { success = true, error_message = error });
            }

            JsonResult resp = (JsonResult)base.Validate_Credentials(userlogin);

            ValidateCredentialsResponse response = (ValidateCredentialsResponse)resp.Value;

            if (response.success)
            {       
                
                return Ok(new { success = true, message = "Success"});
            }
            else
            {
                return BadRequest(new { success = false, message = new Responses.Messages.Language().GetMessage(userlogin.language, "InvalidCred") });
            }
            
        }

    }
}