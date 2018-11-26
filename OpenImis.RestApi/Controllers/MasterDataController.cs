using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.RestApi.Protocol;

namespace OpenImis.RestApi.Controllers
{
	[ApiVersion("1")]
	[Authorize(Roles = "IMISAdmin, EnrollmentOfficer")]
	[Route("api/master")]
	[ApiController]
	[EnableCors("AllowSpecificOrigin")]
	public class MasterDataController : Controller
    {
		private readonly IImisModules _imisModules;


		public MasterDataController(IImisModules imisModules)
		{
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
		[HttpGet]
		[ProducesResponseType(typeof(GetMasterDataResponse), 200)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetAllLocations()
		{
			GetMasterDataResponse masterDataResponse = new GetMasterDataResponse()
			{
				Locations = await _imisModules.GetMasterDataManagementModule().GetLocationLogic().GetAllLocations(),
				FamilyTypes = await _imisModules.GetMasterDataManagementModule().GetFamilyTypeLogic().GetAllFamilyTypes(),
				ConfirmationTypes = await _imisModules.GetMasterDataManagementModule().GetConfirmationTypeLogic().GetAllConfirmationTypes(),
				EducationLevels = await _imisModules.GetMasterDataManagementModule().GetEducationLevelLogic().GetAllEducationLevels(),
				GenderTypes = await _imisModules.GetMasterDataManagementModule().GetGenderTypeLogic().GetAllGenderTypes(),
				RelationTypes = await _imisModules.GetMasterDataManagementModule().GetRelationTypeLogic().GetAllRelationTypes(),
				ProfessionTypes = await _imisModules.GetMasterDataManagementModule().GetProfessionTypeLogic().GetAllProfessionTypes(),
				IdentificationTypes = await _imisModules.GetMasterDataManagementModule().GetIdentificationTypeLogic().GetAllIdentificationTypes(),
			};

			return Ok(masterDataResponse);
		}

	}
}