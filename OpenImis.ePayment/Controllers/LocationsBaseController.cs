using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenImis.ePayment.Data;
using OpenImis.ePayment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace OpenImis.ePayment.Controllers
{

    public abstract class LocationsBaseController : Controller
    {
        private ImisBaseLocations locations;

        public LocationsBaseController(IConfiguration configuration)
        {
            locations = new ImisBaseLocations(configuration);
        }

        [HttpPost]
        [Route("api/Locations/GetOfficerVillages")]
        public virtual IActionResult GetOfficerVillages([FromBody]EOModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            
            var response = locations.GetOfficerVillages(model.enrollment_officer_code);
            return Json(response);

        }
    }
}