using ImisRestApi.Chanels.Sms;
using ImisRestApi.Data;
using ImisRestApi.Models;
using ImisRestApi.Models.Sms;
using ImisRestApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImisRestApi.Controllers
{
    [Authorize]
    public class FamiliesController : Controller
    {
        private ImisFamily family;
        private IConfiguration config;
        public FamiliesController(IConfiguration configuration)
        {
            config = configuration;
            family = new ImisFamily(configuration);
        }
        // GET api/Families
        [HttpGet]
        [Route("api/Families/Get_Family")]
        public IActionResult Get(string insureeNumber)
        {
            DataMessage response;
            try
            {
                if (insureeNumber != null || insureeNumber.Length != 0)
                {
                    var data = family.Get(insureeNumber);

                    if (data.Rows.Count > 0)
                    {
                        response = new GetFamilyResponse(0, false, data).Message;

                    }
                    else
                    {
                        response = new GetFamilyResponse(2, false).Message;
                    }
                }
                else
                {
                    response = new GetFamilyResponse(1, false).Message;
                }
                
            }
            catch (Exception e)
            {
                response = new GetFamilyResponse(e).Message;
                
            }
            

            return Json(response);
        }

        // GET api/Families/5
        [HttpGet]
        [Route("api/Families/Get_Member_Family")]
        public IActionResult Get_Member_Family(string insureeNumber, int order)
        {
           
           
            DataMessage response;
            try
            {
                if (insureeNumber != null || insureeNumber.Length != 0)
                {
                    var data = family.GetMamber(insureeNumber, order);

                    if (data.Rows.Count > 0)
                    {
                        response = new GetMemberFamilyResponse(0, false, data).Message;

                    }
                    else
                    {
                        response = new GetMemberFamilyResponse(2, false).Message;
                    }
                }
                else
                {
                    response = new GetMemberFamilyResponse(1, false).Message;
                }

            }
            catch (Exception e)
            {
                response = new GetMemberFamilyResponse(e).Message;

            }
            return Json(response);
        }

        [HttpPost]
        [Route("api/Families/Enter_Family")]
        public IActionResult Enter_Family([FromBody]Family model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = family.AddNew(model);

            return Json(response);

        }

        [HttpPost]
        [Route("api/Families/Enter_Member_Family")]
        public IActionResult Enter_Member_Family([FromBody]FamilyMamber model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = family.AddMamber(model);

            return Json(response);

        }

        [HttpPost]
        [Route("api/Families/Edit_Family")]
        public IActionResult Edit_Family([FromBody]Family model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = family.Edit(model);

            return Json(response);

        }

        [HttpPost]
        [Route("api/Families/Edit_Member_Family")]
        public IActionResult Edit_Member_Family([FromBody]FamilyMamber model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = family.EditMamber(model);

            return Json(response);

        }

        [HttpPost]
        [Route("api/Families/Delete_Member_Family")]
        public IActionResult Delete_Member_Family([FromBody]string insureeNumber)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = family.DeleteMamber(insureeNumber);

            return Json(response);

        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("api/Enquire")]
        public async Task<IActionResult> Enquire([FromBody]Enquire model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = family.Enquire(model.chfid);
            var jsonResponse = JsonConvert.SerializeObject(response);
            List<EnquireResponse> resp = JsonConvert.DeserializeObject<List<EnquireResponse>>(jsonResponse);

            string msgString = "IMIS Insuree:"+ resp.FirstOrDefault().insureeName;

            foreach (var item in resp)
            {
                msgString += item.productCode + " : " + item.status;
            }

            List<SmsContainer> message = new List<SmsContainer>();
            message.Add(new SmsContainer() { Message = msgString, Recepients = "+255767057265" });

            ImisSms sms = new ImisSms(config);
            string test = await sms.PushSMS(message);

            return Json(new { status = true,sms_reply=true,sms_text = resp });

        }
    }
}
