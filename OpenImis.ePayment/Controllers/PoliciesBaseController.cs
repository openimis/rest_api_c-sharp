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
    [Authorize]
    public abstract class PoliciesBaseController : Controller
    {
        private ImisPolicy policies;

        public PoliciesBaseController(IConfiguration configuration)
        {
            policies = new ImisPolicy(configuration);
        }

        [HttpPost]
        [Route("api/Policies/Enter_Policy")]
        public virtual IActionResult Enter_Policy([FromBody]USSDPolicy model)
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
                policies.UserId = Convert.ToInt32(iden.Value);
            }
            catch (Exception)
            {
                policies.UserId = -1;
            }

            var response = policies.Enter(model);
            return Json(response);

        }

        [HttpPost]
        [Route("api/Policies/Renew_Policy")]
        public virtual IActionResult Renew_Policy([FromBody]USSDPolicy model)
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
                policies.UserId = Convert.ToInt32(iden.Value);
            }
            catch (Exception)
            {
                policies.UserId = -1;
            }

            var response = policies.Renew(model);

            return Json(response);

        }

        [HttpPost]
        [Route("api/Policies/Get_Commissions")]
        public virtual IActionResult Get_Commissions([FromBody]USSDGetCommissionInputs model)
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
                policies.UserId = Convert.ToInt32(iden.Value);
            }
            catch (Exception)
            {
                policies.UserId = -1;
            }

            var response = policies.GetCommissions(model);

            return Json(response);

        }
    }
}
