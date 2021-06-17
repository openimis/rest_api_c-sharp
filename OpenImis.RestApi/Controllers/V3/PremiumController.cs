using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.PremiumModule.Models;
using OpenImis.Security.Security;

namespace OpenImis.RestApi.Controllers.V3
{
    [ApiVersion("3")]
    [Authorize]
    [Route("api/premium/")]
    [ApiController]
    public class PremiumController : Controller
    {
        private readonly IImisModules _imisModules;

        public PremiumController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HasRights(Rights.PolicySearch)]
        [Route("receipt/")]
        [HttpPost]
        public IActionResult Get([FromBody]ReceiptRequestModel receipt)
        {
            bool response;

            try
            {
                response = _imisModules.GetPremiumModule().GetPremiumLogic().Get(receipt);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            return Ok(response);
        }
    }
}