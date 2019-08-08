using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV2;
using OpenImis.ModulesV2.ClaimModule.Models.RegisterClaim;

namespace OpenImis.RestApi.Controllers.V2
{
    [ApiVersion("2")]
    [Authorize]
    [Route("api/claim")]
    [ApiController]
    public class ClaimController : Controller
    {
        private readonly IImisModules _imisModules;

        public ClaimController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        //[HasRights(Rights.ClaimAdd)]
        [HttpPost]
        public IActionResult Create([FromBody] Claim claim)
        {
            int response;

            try
            {
                response = _imisModules.GetClaimModule().GetClaimLogic().RegisterClaim(claim);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            return Ok(response);
        }
    }
}