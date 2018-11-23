using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenImis.RestApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/values")]
	[ApiController]
	public class ValuesControllerV1 : Controller
    {
        // GET api/values
        [Authorize(Roles = "EnrollmentOfficer")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
			var result = new string[] { "value1", "value2", User.Identity.Name };
			return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(id);
        }

        // POST api/values
   //     [HttpPost]
   //     public async Task<IActionResult> Post([FromBody]string value)
   //     {
			//return Ok();
   //     }

   //     // PUT api/values/5
   //     [HttpPut("{id}")]
   //     public async Task<IActionResult> Put(int id, [FromBody]string value)
   //     {
			//return Ok();
   //     }

   //     // DELETE api/values/5
   //     [HttpDelete("{id}")]
   //     public async Task<IActionResult> Delete(int id)
   //     {
			//return Ok();
   //     }
    }
}
