using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

		// GET api/values
		[HttpGet]
        public async Task<IActionResult> GetAllFamilies([FromQuery]int page = 1, [FromQuery]int numberPerPage = 0)
        {
			//var result = new string[] { "value1", "value2", User.Identity.Name };

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
        public async Task<IActionResult> Post([FromBody]string value)
        {
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
