using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Controllers;
using ImisRestApi.Escape.Models;
using ImisRestApi.Models;
using ImisRestApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ImisRestApi.Escape
{
    public class ContributionsController : ContributionsBaseController
    {
        public ContributionsController(IConfiguration configuration) : base(configuration)
        {

        }

        [NonAction]
        public override IActionResult Enter_Contribution([FromBody] Contribution model)
        {
            return base.Enter_Contribution(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Contributions/Enter_Contribution")]
        public IActionResult Enter_ContributionUssd([FromBody] ChfContribution model)
        {
            model.PaymentDate = DateTime.UtcNow.ToString("yyyy/MM/dd");
            model.PaymentType = "M";

            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;
                return BadRequest(new { esuccess = true, message = error });
            }

            JsonResult resp = (JsonResult)base.Enter_Contribution(model);
            DataMessage message = (DataMessage)resp.Value;

            if (!message.ErrorOccured)
            {
                var jsonstring = (string)message.Data;
                List<ChfContributionResponse> respData = JsonConvert.DeserializeObject<List<ChfContributionResponse>>(jsonstring);

                if (respData.FirstOrDefault().PolicyStatus == 2) {
                    return Ok(new { success = false, message = message.MessageValue });
                }
                else
                {
                    return Ok(new { success = false, message = new Responses.Messages.Language().GetMessage(model.language, "PolicyNotActivated") });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = message.MessageValue });
            }
           
        }
    }
}