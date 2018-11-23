using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.Modules.InsureeManagementModule.Models;
using OpenImis.Modules.InsureeManagementModule.Protocol;

namespace OpenImis.RestApi.Controllers
{
    [ApiVersion("1")]
    [Authorize(Roles = "IMISAdmin, EnrollmentOfficer")]
    [Route("api/family")]
	[ApiController]
	public class FamilyControllerV1 : Controller
    {
		private readonly IImisModules _imisModules;


		public FamilyControllerV1(IImisModules imisModules)
		{
			_imisModules = imisModules;
		}

		/// <summary>
		/// Get the list of the Families
		/// </summary>
		/// <param name="page">Number of the page</param>
		/// <param name="resultsPerPage">Number of families per request/page</param>
		/// <returns>The list of families</returns>
		/// <remarks>
		/// ### REMARKS ###
		/// The following codes are returned
		/// - 200 - The list of the families 
		/// - 400 - The request is invalid
		/// - 401 - The token is invalid
		/// </remarks>
		/// <response code="200">Returns the list of families</response>
		/// <response code="400">If the request is incomplete</response>      
		/// <response code="401">If the token is missing, is wrong or expired</response>      
		[HttpGet]
		[ProducesResponseType(typeof(GetFamiliesResponse), 200)]
		[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetFamilies([FromQuery]int page = 1, [FromQuery]int resultsPerPage = 20)
        {
			GetFamiliesResponse families;
			families = await _imisModules.GetInsureeManagementModule().GetFamilyLogic().GetFamilies(page, resultsPerPage);

			return Ok(families);
        }

		// GET api/ws/family/00001
		[HttpGet("insuree/{insureeId}", Name = "GetFamilyByInsureeId")]
        public async Task<IActionResult> GetFamilyByInsureeId(string insureeId)
        {
			FamilyModel familyModel;

			try
			{
				familyModel = await _imisModules.GetInsureeManagementModule().GetFamilyLogic().GetFamilyByInsureeId(insureeId);
			}
			catch (ValidationException e)
			{
				return BadRequest(new { error = new { message = e.Message, value = e.Value } });
			}

			if (familyModel==null)
			{
				return NotFound();
			}

			return Ok(familyModel);
        }


		[HttpGet("{familyId}", Name = "GetFamilyByFamilyId")]
		public async Task<IActionResult> GetFamilyByFamilyId(int familyId)
		{
			FamilyModel familyModel;

			try
			{
				familyModel = await _imisModules.GetInsureeManagementModule().GetFamilyLogic().GetFamilyByFamilyId(familyId);
			}
			catch (ValidationException e)
			{
				return BadRequest(new { error = new { message = e.Message, value = e.Value } });
			}

			if (familyModel == null)
			{
				return NotFound();
			}

			return Ok(familyModel);
		}


		// POST api/values
		[HttpPost]
        public async Task<IActionResult> AddNewFamily([FromBody]FamilyModel family)
        {
			FamilyModel newFamily;
			try
			{
				newFamily = await _imisModules.GetInsureeManagementModule().GetFamilyLogic().AddFamily(family);
			}
			catch (ValidationException e)
			{
				return BadRequest(new { error = new { message = e.Message, value = e.Value } });
			}
			catch (Exception e)
			{
				return BadRequest(new { error = new { message = e.Message, source = e.Source, trace = e.StackTrace } });
			}

			return Created(new Uri(Url.Link("GetFamilyByFamilyId", new { familyId = newFamily.FamilyId})), newFamily);
        }

        // PUT api/values/5
        [HttpPut("{familyId}")]
        public async Task<IActionResult> UpdateFamily(int familyId, [FromBody]FamilyModel family)
        {
			FamilyModel updatedFamily;
			try
			{
				updatedFamily = await _imisModules.GetInsureeManagementModule().GetFamilyLogic().UpdateFamilyAsync(familyId, family);
			}
			catch (ValidationException e)
			{
				return BadRequest(new { error = new { message = e.Message, value = e.Value } });
			}
			catch (Exception e)
			{
				return BadRequest(new { error = new { message = e.Message, source = e.Source, trace = e.StackTrace } });
			}

			return Ok(updatedFamily);
		}

        // DELETE api/values/5
        [HttpDelete("{familyId}")]
        public async Task<IActionResult> Delete(int familyId)
        {

			try
			{
				await _imisModules.GetInsureeManagementModule().GetFamilyLogic().DeleteFamilyAsync(familyId);
			}
			catch (ValidationException e)
			{
				return BadRequest(new { error = new { message = e.Message, value = e.Value } });
			}
			catch (Exception e)
			{
				return BadRequest(new { error = new { message = e.Message, source = e.Source, trace = e.StackTrace } });
			}

			return Accepted();
        }
    }
}
