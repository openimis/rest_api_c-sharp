using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.Modules.CoverageModule.Helpers;
using OpenImis.Modules.CoverageModule.Models;
using OpenImis.RestApi.Security;

namespace OpenImis.RestApi.Controllers
{
    [Authorize]
    [Route("api/")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class CoverageController : Controller
    {
        private readonly IImisModules _imisModules;
        public CoverageController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HasRights(Rights.InsureeSearch)]
        [HttpGet]
        [Route("Coverage/Get_Coverage")]
        public virtual IActionResult Get(string InsureeNumber)
        {
            // Temporary HTTP 400
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error_occured = true, error_message = "1:Wrong format or missing insurance number of insuree" });
            }

            //if (new ValidationBase().InsureeNumber(InsureeNumber) != ValidationResult.Success)
            //{
            //    return BadRequest(new { error_occured = true, error_message = "1:Wrong format or missing insurance number of insuree" });
            //}

            DataMessage response;
            try
            {
                if (InsureeNumber != null || InsureeNumber.Length != 0)
                {
                    var data = _imisModules.GetCoverageModule().GetCoverageLogic().Get(InsureeNumber);

                    if (data != null)
                    {
                        response = new GetCoverageResponse(0, false, data, 0).Message;
                    }
                    else
                    {
                        response = new GetCoverageResponse(2, true, 0).Message;
                    }
                }
                else
                {
                    response = new GetCoverageResponse(1, true, 0).Message;
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