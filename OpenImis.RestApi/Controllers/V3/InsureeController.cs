using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV3;
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

        public InsureeController(IImisModules imisModules)
        {
            _imisModules = imisModules;
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
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
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
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            if (getEnquireModel == null)
            {
                return NotFound();
            }

            return Ok(getEnquireModel);
        }
    }
}