using OpenImis.ePayment.Data;
using OpenImis.ePayment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;

namespace OpenImis.ePayment.Controllers
{
    /*
    [Authorize]
    public abstract class ContributionsBaseController : Controller
    {
        private ImisContribution contribution;

        public ContributionsBaseController(IConfiguration configuration)
        {
            contribution = new ImisContribution(configuration);
        }

        [HttpPost]
        [Route("api/Contributions/Enter_Contribution")]
        public virtual IActionResult Enter_Contribution([FromBody]Contribution model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var iden = identity.FindFirst("UserId");

            try
            {
                contribution.UserId = Convert.ToInt32(iden.Value);
            }
            catch (Exception e)
            {
                contribution.UserId = -1;
            }
            
            var response = contribution.Enter(model);

            return Json(response);

            
        }

    }*/
}
