using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.PremiumModule.Models;
using OpenImis.RestApi.Util.ErrorHandling;
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
        private readonly ILogger _logger;

        public PremiumController(IImisModules imisModules, ILoggerFactory loggerFactory)
        {
            _imisModules = imisModules;
            _logger = loggerFactory.CreateLogger<PremiumController>();
        }

        [HasRights(Rights.PolicySearch)]
        [Route("receipt/")]
        [HttpPost]
        public IActionResult Get([FromBody] ReceiptRequestModel receipt)
        {
            bool response;

            try
            {
                response = _imisModules.GetPremiumModule().GetPremiumLogic().Get(receipt);
            }
            catch (ValidationException e)
            {
                throw new BusinessException(e.Message);
            }

            return Ok(response);
        }
    }
}