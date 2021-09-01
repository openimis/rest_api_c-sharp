using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.InsureeModule.Models;
using OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels;
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

        public FamilyController(IImisModules imisModules)
        {
            _imisModules = imisModules;
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
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            if (familyModel == null)
            {
                return NotFound();
            }

            return Ok(familyModel);
        }

        [HasRights(Rights.FamilyAdd)]
        [HttpPost]
        public IActionResult Create([FromBody] EnrollFamilyModel model)
        {
            NewFamilyResponse response;

            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

                int userId = _imisModules.GetInsureeModule().GetFamilyLogic().GetUserIdByUUID(userUUID);
                int officerId = _imisModules.GetInsureeModule().GetFamilyLogic().GetOfficerIdByUserUUID(userUUID);

                response = _imisModules.GetInsureeModule().GetFamilyLogic().Create(model, userId, officerId);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            // If the response is 1001 then Family uploaded successfully but it failed to map control number entries
            return Ok(response);
        }
    }
}