using OpenImis.ePayment.Data;
using OpenImis.ePayment.Models;
using OpenImis.ePayment.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Controllers
{
    [Authorize]
    public abstract class ValidateBaseController : Controller
    {
        private ImisValidate validate;

        public ValidateBaseController(IConfiguration configuration)
        {
            validate = new ImisValidate(configuration);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Validate/Credentials")]
        public virtual IActionResult Validate_Credentials([FromBody]UserLogin userlogin)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            ValidateCredentialsResponse response = new ValidateCredentialsResponse();

            try
            {

                var user = validate.FindUser(userlogin.UserID, userlogin.Password);

                if (user != null)
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
