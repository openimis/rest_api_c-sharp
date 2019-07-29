using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV1.CoverageModule.Helpers;
using OpenImis.ModulesV1.CoverageModule.Models;
using OpenImis.RestApi.Security;
using OpenImis.ModulesV1;
using OpenImis.ModulesV1.Helpers.Validators;

namespace OpenImis.RestApi.Controllers.V1
{
    [ApiVersion("1")]
    [Authorize]
    [Route("api/")]
    [ApiController]
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
            if (new Validation().InsureeNumber(InsureeNumber) != ValidationResult.Success)
            {
                return BadRequest(new { error_occured = true, error_message = "1:Wrong format or missing insurance number of insuree" });
            }

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