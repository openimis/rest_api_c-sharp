using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.InsureeModule.Models;
using OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels;
using OpenImis.ModulesV3.Utils;
using OpenImis.RestApi.Util.ErrorHandling;
using OpenImis.Security.Security;

namespace OpenImis.RestApi.Controllers.V3
{
    [ApiVersion("3")]
    [Authorize]
    [Route("api/family/")]
    [ApiController]
    public class FamilyController : Controller
    {
        private readonly IImisModules _imisModules;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public FamilyController(IImisModules imisModules, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _imisModules = imisModules;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<FamilyController>();
        }

        // TODO Remove from Model ID for Family and Insuree after changes in SP
        [HasRights(Rights.FamilySearch)]
        [HttpGet]
        [Route("{chfid}")]
        public IActionResult GetByCHFID(string chfid)
        {
            FamilyModel familyModel = new FamilyModel();

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());
                familyModel = _imisModules.GetInsureeModule().GetFamilyLogic().GetByCHFID(chfid, userUUID);
            }
            catch (ValidationException e)
            {
                throw new BusinessException(e.Message);
            }

            if (familyModel == null)
            {
                return NotFound();
            }

            return Ok(familyModel);
        }

        [HasRights(Rights.FamilyAdd)]
        [HttpPost]
        public IActionResult Create([FromBody] EnrolFamilyModel model)
        {

            // Save the payload in to a folder before performing any other tasks
            //  This will help us later to debug in case of any queries
            Guid userUUID;
            int officerId;

            try
            {

                var JSON = JsonConvert.SerializeObject(model);
                var dateFolder = DateTime.Now.Year.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Month.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Day.ToString() + Path.DirectorySeparatorChar;
                var JsonDebugFolder = _configuration["AppSettings:JsonDebugFolder"] + Path.DirectorySeparatorChar + dateFolder;

                var hof = model.Family.Select(x => x.HOFCHFID).FirstOrDefault();
                userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());
                officerId = _imisModules.GetInsureeModule().GetFamilyLogic().GetOfficerIdByUserUUID(userUUID);

                var JsonFileName = string.Format("{0}_{1}_{2}.json", hof, officerId.ToString(), DateTime.Now.ToString(DateTimeFormats.FileNameDateTimeFormat));

                if (!Directory.Exists(JsonDebugFolder)) Directory.CreateDirectory(JsonDebugFolder);

                System.IO.File.WriteAllText(JsonDebugFolder + JsonFileName, JSON);

            }
            catch (Exception e)
            {

                throw new BusinessException(e.Message);
            }
            NewFamilyResponse response;

            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            try
            {


                int userId = _imisModules.GetInsureeModule().GetFamilyLogic().GetUserIdByUUID(userUUID);


                response = _imisModules.GetInsureeModule().GetFamilyLogic().Create(model, userId, officerId);
            }
            catch (ValidationException e)
            {
                throw new BusinessException(e.Message);
            }

            // If the response is 1001 then Family uploaded successfully but it failed to map control number entries
            return Ok(response);
        }
    }
}