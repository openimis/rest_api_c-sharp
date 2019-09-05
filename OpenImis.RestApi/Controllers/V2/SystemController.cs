using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV2;

namespace OpenImis.RestApi.Controllers.V2
{
    [ApiVersion("2")]
    [Authorize]
    [Route("api/system/")]
    [ApiController]
    public class SystemController : Controller
    {
        private readonly IImisModules _imisModules;

        public SystemController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("apkversion/{APK_Name}")]
        public IActionResult Get(string APK_Name)
        {
            string response = _imisModules.GetSystemModule().GetSystemLogic().Get(APK_Name);

            if (response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}