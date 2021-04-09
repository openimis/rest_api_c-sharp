using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenImis.Modules;
using OpenImis.Modules.MasterDataModule.Models;
using ImisRestApi.Security;

namespace ImisRestApi.Controllers
{
    //[ApiVersion("2")]
    [Authorize]
    [Route("api/")]
    //[ApiController]
    public class MasterDataController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IImisModules _imisModules;

        public MasterDataController(IConfiguration configuration, IImisModules imisModules)
        {
            _configuration = configuration;
            _imisModules = imisModules;
        }

        /// <summary>
		/// Get the Master Data
		/// </summary>
		/// <returns>The Master Data</returns>
		/// <remarks>
		/// ### REMARKS ###
		/// The following codes are returned
		/// - 200 - The Master Data 
		/// - 401 - The token is invalid
		/// </remarks>
		/// <response code="200">Returns the list of Locations</response>
		/// <response code="401">If the token is missing, is wrong or expired</response>      
        //[HasRights(Rights.ExtractMasterDataDownload)]
        [AllowAnonymous]
        [HttpGet]
        [Route("master")]
        [ProducesResponseType(typeof(GetMasterDataResponse), 200)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult GetMasterData()
        {
            MasterDataModel masterData = _imisModules.GetMasterDataModule().GetMasterDataLogic().GetMasterData();

            return Ok(masterData);
        }
    }
}
