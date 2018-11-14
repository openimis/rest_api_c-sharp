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
		/// <param name="numberPerPage">Number of families per request/page</param>
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
		[ProducesResponseType(typeof(FamilyModel[]), 200)]
		[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> GetAllFamilies([FromQuery]int page = 1, [FromQuery]int numberPerPage = 20)
        {
			FamilyModel[] families;
			families = await _imisModules.GetInsureeManagementModule().GetFamilyLogic().GetAllFamilies(page, numberPerPage);

			return Ok(families);
        }

		// GET api/ws/family/00001
		[HttpGet("{insureeId}")]
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

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FamilyModel family)
        {
			try
			{
				await _imisModules.GetInsureeManagementModule().GetFamilyLogic().AddFamily(family);
			}
			catch (ValidationException e)
			{
				return BadRequest(new { error = new { message = e.Message, value = e.Value } });
			}

			return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]string value)
        {
			return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
			return Ok();
        }
    }
}
