using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ImisRestApi.Chanels.Sms;
using ImisRestApi.Controllers;
using ImisRestApi.Escape.Models;
using ImisRestApi.Logic;
using ImisRestApi.Models;
using ImisRestApi.Models.Sms;
using ImisRestApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ImisRestApi.Controllers
{
    public class CoverageController : CoverageBaseController
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CoverageController(IConfiguration configuration, IHostingEnvironment hostingEnvironment) : base(configuration)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [NonAction]
        public override IActionResult Get(string InsureeNumber)
        {
            return base.Get(InsureeNumber);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/Coverage/Get_Coverage")]
        public async Task<IActionResult> GetChf([FromBody]ChfGetCoverageModel model)
        {
            if (new ValidationBase().InsureeNumber(model.insureeNumber) != ValidationResult.Success)
            {
                return BadRequest(new { success = false, message = "1:Wrong format or missing insurance number of insuree" });
            }

            JsonResult resp = (JsonResult)base.Get(model.insureeNumber);
            DataMessage message = (DataMessage)resp.Value;

            var response = new GetCoverageResponse(message.Code, message.ErrorOccured, model.language).Message;

            if (!message.ErrorOccured) {

                var coverage = new ChfCoverage();
                try
                {
                    var covstring = JsonConvert.SerializeObject(message.Data);
                    coverage = JsonConvert.DeserializeObject<ChfCoverage>(covstring);

                }
                catch (Exception)
                {

                    throw;
                }
               
                Language language = Language.Secondary;

                if (model.language == 2) {
                    language = Language.Primary;
                }

                ImisSms sms = new ImisSms(_configuration, _hostingEnvironment, language);
                
                var txtmsgTemplate = sms.GetMessage("EnquireInformSms");

                
                var txtmsg = string.Format(txtmsgTemplate,
                         coverage.OtherNames,
                         coverage.LastNames,
                         coverage.BirthDate,
                         model.insureeNumber,
                         coverage.CoverageProducts.FirstOrDefault().ProductCode,
                         coverage.CoverageProducts.FirstOrDefault().Status,
                         coverage.CoverageProducts.FirstOrDefault().EffectiveDate,
                         coverage.CoverageProducts.FirstOrDefault().ExpiryDate
                         );

                for(int i = 1; i > coverage.CoverageProducts.Count; i++)
                {
                    var cov = coverage.CoverageProducts[i];

                    var txt = string.Format(txtmsgTemplate,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        cov.ProductCode,
                        cov.Status,
                        cov.EffectiveDate,
                        cov.ExpiryDate
                        );

                    txtmsg += ":-" + txtmsg.Split(":-")[1];

                }


                sms.QuickSms(txtmsg, model.msisdn);

                return Ok(new { success = true, message = response.MessageValue });
            }
            else
            {
                return BadRequest(new { success = false, message = response.MessageValue });
            }
           
        }
    }
}