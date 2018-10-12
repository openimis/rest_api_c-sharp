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
    public class ContributionsController : Controller
    {
        private ImisContribution contribution;

        public ContributionsController(IConfiguration configuration)
        {
            contribution = new ImisContribution(configuration);
        }

        [HttpPost]
        [Route("api/Contributions/Enter_Contribution")]
        public IActionResult Enter_Contribution([FromBody]Contribution model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            contribution.UserId = Convert.ToInt32(identity.FindFirst("UserId").Value);

            var response = contribution.Enter(model);

            return Json(response);

            
        }

    }
}
