using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenImis.Modules;
using OpenImis.Modules.InsureeModule.Helpers;
using OpenImis.Modules.InsureeModule.Models;

namespace OpenImis.RestApi.Controllers
{
    [Authorize]
    [Route("api/")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class FamilyController : Controller
    {
        private readonly IImisModules _imisModules;
        public FamilyController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HttpGet]
        [Route("Families/Get_Family")]
        public IActionResult Get(string insureeNumber)
        {
            DataMessage response;
            try
            {
                if (insureeNumber != null || insureeNumber.Length != 0)
                {
                    var data = _imisModules.GetInsureeModule().GetFamilyLogic().Get(insureeNumber);

                    if (data.Count > 0)
                    {
                        response = new GetFamilyResponse(0, false, data, 0).Message;
                    }
                    else
                    {
                        response = new GetFamilyResponse(2, false, 0).Message;
                    }
                }
                else
                {
                    response = new GetFamilyResponse(1, true, 0).Message;
                }
            }
            catch (Exception e)
            {
                response = new GetFamilyResponse(e).Message;
            }

            return Json(response);
        }

        [HttpGet]
        [Route("Families/Get_Member_Family")]
        public IActionResult Get_Member_Family(string insureeNumber, int order)
        {
            DataMessage response;
            try
            {
                if (insureeNumber != null || insureeNumber.Length != 0)
                {
                    var data = _imisModules.GetInsureeModule().GetFamilyLogic().GetMember(insureeNumber, order);

                    if (data.Count > 0)
                    {
                        response = new GetMemberFamilyResponse(0, false, data, 0).Message;
                    }
                    else
                    {
                        response = new GetMemberFamilyResponse(2, true, 0).Message;
                    }
                }
                else
                {
                    response = new GetMemberFamilyResponse(1, true, 0).Message;
                }
            }
            catch (Exception e)
            {
                response = new GetMemberFamilyResponse(e).Message;
            }

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return Json(response, serializerSettings);
        }

        [HttpPost]
        [Route("Families/Enter_Family")]
        public IActionResult Enter_Family([FromBody]FamilyModelv3 model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //_imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(Convert.ToInt32(identity.FindFirst("UserId").Value));

            // Temporary
            var userId = 1;
            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetFamilyLogic().AddNew(model);

            return Json(response);
        }

        [HttpPost]
        [Route("Families/Edit_Family")]
        public IActionResult Edit_Family([FromBody]EditFamily model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            int userId = Convert.ToInt32(HttpContext.User.Claims
                .Where(w => w.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetFamilyLogic().Edit(model);

            return Json(response);
        }

        [HttpPost]
        [Route("Families/Enter_Member_Family")]
        public IActionResult Enter_Member_Family([FromBody]FamilyMember model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            int userId = Convert.ToInt32(HttpContext.User.Claims
                .Where(w => w.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetFamilyLogic().AddMember(model);

            return Json(response);
        }

        [HttpPost]
        [Route("Families/Edit_Member_Family")]
        public IActionResult Edit_Member_Family([FromBody]EditFamilyMember model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { error_occured = true, error_message = error });
            }

            int userId = Convert.ToInt32(HttpContext.User.Claims
                .Where(w => w.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetFamilyLogic().EditMember(model);

            return Json(response);
        }

        [HttpPost]
        [Route("Families/Delete_Member_Family")]
        public IActionResult Delete_Member_Family([FromBody]string insureeNumber)
        {
            //if (new ValidationBase().InsureeNumber(insureeNumber) != ValidationResult.Success)
            //{
            //    return BadRequest(new { error_occured = true, error_message = "1:Wrong format or missing insurance number of insuree" });
            //}

            int userId = Convert.ToInt32(HttpContext.User.Claims
                .Where(w => w.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            _imisModules.GetInsureeModule().GetFamilyLogic().SetUserId(userId);

            var response = _imisModules.GetInsureeModule().GetFamilyLogic().DeleteMember(insureeNumber);

            return Json(response);
        }
    }
}