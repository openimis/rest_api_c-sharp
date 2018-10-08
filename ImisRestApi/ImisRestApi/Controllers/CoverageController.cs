using ImisRestApi.Data;
using ImisRestApi.Logic;
using ImisRestApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace ImisRestApi.Controllers
{
    [Authorize]
    public class CoverageController : Controller
    {
        private ImisCoverage coverage;

        public CoverageController(IConfiguration configuration)
        {
            coverage = new ImisCoverage(configuration);
        }
        // GET api/Coverage
        [HttpGet]
        [Route("api/Coverage/Get_Coverage")]
        public IActionResult Get(string InsureeNumber)
        {
            if (new ValidationBase().InsureeNumber(InsureeNumber) != ValidationResult.Success)
            {
                return BadRequest(new { error_occured = true, error_message = "1:Wrong format or missing insurance number of insuree" });
            }

            DataMessage response;
            try
            {
                
                if (InsureeNumber != null || InsureeNumber.Length != 0)
                {
                    var data = coverage.Get(InsureeNumber);

                    if (data.Rows.Count > 0)
                    {
                        
                        response = new GetCoverageResponse(0, false, data).Message;

                    }
                    else
                    {
                        response = new GetCoverageResponse(2, true).Message;
                    }
                }
                else
                {
                    response = new GetCoverageResponse(1, true).Message;
                }

              
            }      
            catch (SqlException e)
            {
                response = new GetCoverageResponse(e).Message;
            }
            catch (Exception e)
            {
                response = new GetCoverageResponse(e).Message;
                
            }

            return Json(response);
        }
    }
}
