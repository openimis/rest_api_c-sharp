using ImisRestApi.Data;
using ImisRestApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace ImisRestApi.Controllers
{
    [Authorize]
    public class PoliciesController : Controller
    {
        private ImisPolicy policies;

        public PoliciesController(IConfiguration configuration)
        {
            policies = new ImisPolicy(configuration);
        }

        [HttpPost]
        [Route("api/Policies/Enter_Policy")]
        public IActionResult Enter_Policy([FromBody]Policy model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            policies.UserId = Convert.ToInt32(identity.FindFirst("UserId").Value);

            var response = policies.Enter(model);
            return Json(response);

        }

        [HttpPost]
        [Route("api/Policies/Renew_Policy")]
        public IActionResult Renew_Policy([FromBody]Policy model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            policies.UserId = Convert.ToInt32(identity.FindFirst("UserId").Value);

            var response = policies.Renew(model);

            return Json(response);

        }
    }
}
