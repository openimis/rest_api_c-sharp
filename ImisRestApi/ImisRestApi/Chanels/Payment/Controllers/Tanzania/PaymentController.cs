using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ImisRestApi.Data;

namespace ImisRestApi.Chanels
{
    public partial class PaymentController : Controller
    {
        [HttpPost]
        [Route("api/GetControlNumber")]
        public async Task<IActionResult> ControlNumber([FromBody]IntentOfPay intent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            return View();
        }
    }
}