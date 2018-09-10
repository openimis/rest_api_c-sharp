using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.Modules.WSModule.Models;

namespace OpenImis.RestApi.Controllers
{
    [ApiVersion("1")]
    [Authorize(Roles = "IMISAdmin, EnrollmentOfficer")]
    [Route("api/ws")]
	[ApiController]
	public class WSControllerV1 : Controller
    {
		private readonly IImisModules _imisModules;


		public WSControllerV1(IImisModules imisModules)
		{
			_imisModules = imisModules;
		}

		// GET api/values
		[HttpGet]
        public async Task<IActionResult> Get()
        {
			var result = new string[] { "value1", "value2", User.Identity.Name };
			return Ok(result);
        }

		// GET api/ws/family/00001
		[HttpGet("family/{chfid}")]
        public async Task<IActionResult> Get(string chfid)
        {
			FamilyModel familyModel;

			try
			{
				familyModel = await _imisModules.getWSModule().GetFamilyController().GetFamily(chfid);
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
