using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.Modules.InsureeModule.Models;

namespace OpenImis.RestApi.Controllers
{
    [Authorize]
    [Route("api/")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class ContributionController : Controller
    {
        private readonly IImisModules _imisModules;
        public ContributionController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HttpPost]
        [Route("Contributions/Enter_Contribution")]
        public virtual IActionResult Enter_Contribution([FromBody]Contribution model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            int userId = Convert.ToInt32(HttpContext.User.Claims
                .Where(w => w.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetContributionLogic().Enter(model);

            return Json(response);
        }
    }
}