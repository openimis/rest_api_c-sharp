using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Data;
using ImisRestApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ImisRestApi.Controllers
{
    [Authorize]
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