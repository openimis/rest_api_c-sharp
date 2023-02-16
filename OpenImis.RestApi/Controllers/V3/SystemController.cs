using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenImis.ModulesV2;

namespace OpenImis.RestApi.Controllers.V3
{
    [ApiVersion("3")]
    [Authorize]
    [Route("api/system/")]
    [ApiController]
    public class SystemController : Controller
    {
        private readonly ILogger _logger;
        private readonly IImisModules _imisModules;

        public SystemController(IImisModules imisModules, ILoggerFactory loggerFactory)
        {
            _imisModules = imisModules;
            _logger = loggerFactory.CreateLogger<SystemController>();
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