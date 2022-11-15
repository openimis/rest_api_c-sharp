using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.Helpers;
using OpenImis.ModulesV3.InsureeModule.Models;
using OpenImis.Security.Security;

namespace OpenImis.RestApi.Controllers.V3
{
    [ApiVersion("3")]
    [Authorize]
    [Route("api/insuree/")]
    [ApiController]
    public class InsureeController : Controller
    {
        private readonly IImisModules _imisModules;
        private readonly ILogger _logger;

        public InsureeController(IImisModules imisModules, ILoggerFactory loggerFactory)
        {
            _imisModules = imisModules;
            _logger = loggerFactory.CreateLogger<InsureeController>();
        }

        [HasRights(Rights.InsureeEnquire)]
        [HttpGet]
        [Route("{chfid}")]
        public IActionResult Get(string chfid)
        {
            GetInsureeModel getInsureeModel;

            try
            {
                getInsureeModel = _imisModules.GetInsureeModule().GetInsureeLogic().Get(chfid);
            }
            catch (ValidationException e)
            {
                throw new BusinessException(e.Message);
            }

            if (getInsureeModel == null)
            {
                return NotFound();
            }

            return Ok(getInsureeModel);
        }

        [HasRights(Rights.InsureeEnquire)]
        [HttpGet]
        [Route("{chfid}/enquire")]
        public IActionResult GetEnquire(string chfid)
        {
            GetEnquireModel getEnquireModel;

            try
            {
                getEnquireModel = _imisModules.GetInsureeModule().GetInsureeLogic().GetEnquire(chfid);
            }
            catch (ValidationException e)
            {
                throw new BusinessException(e.Message);
            }

            if (getEnquireModel == null)
            {
                return NotFound();
            }

            return Ok(getEnquireModel);
        }
    }
}