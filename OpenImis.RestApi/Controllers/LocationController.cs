using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.Modules.MasterDataManagementModule.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenImis.RestApi.Controllers
{
	[ApiVersion("1")]
	[Authorize(Roles = "IMISAdmin, EnrollmentOfficer")]
	[Route("api/location")]
	[ApiController]
	public class LocationController : Controller
	{
		private readonly IImisModules _imisModules;


		public LocationController(IImisModules imisModules)
		{
			_imisModules = imisModules;
		}

		/// <summary>
		/// Get the list of Locations
		/// </summary>
		/// <returns>The list of Locations</returns>
		/// <remarks>
		/// ### REMARKS ###
		/// The following codes are returned
		/// - 200 - The list of the families 
		/// - 401 - The token is invalid
		/// </remarks>
		/// <response code="200">Returns the list of Locations</response>
		/// <response code="401">If the token is missing, is wrong or expired</response>      
		[HttpGet]
		[ProducesResponseType(typeof(LocationModel[]), 200)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetAllLocations()
		{
			LocationModel[] locations;
			locations = await _imisModules.GetMasterDataManagementModule().GetLocationLogic().GetAllLocations();

			return Ok(locations);
		}
	}
}
