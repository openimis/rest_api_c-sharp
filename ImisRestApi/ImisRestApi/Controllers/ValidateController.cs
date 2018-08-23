using ImisRestApi.Data;
using ImisRestApi.Models;
using ImisRestApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ImisRestApi.Controllers
{
    [Authorize]
    public class ValidateController : Controller
    {
        private ImisValidate validate;

        public ValidateController(IConfiguration configuration)
        {
            validate = new ImisValidate(configuration);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Validate/Credentials")]
        public IActionResult Validate_Credentials([FromBody]UserLogin userlogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ValidateCredentialsResponse response = new ValidateCredentialsResponse();

            try
            {
                var data = validate.Credentials(userlogin);

                if (data.Rows.Count > 0)
                {
                    response.success = true;
                    response.ErrorOccured = false;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                response.success = false;
                response.ErrorOccured = true;
                
            }
            
            return Json(response);

        }
    }
}
