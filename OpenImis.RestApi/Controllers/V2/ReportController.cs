using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.ModulesV2;
using OpenImis.ModulesV2.ReportModule.Models;
using OpenImis.RestApi.Security;

namespace OpenImis.RestApi.Controllers.V2
{
    [ApiVersion("2")]
    [Authorize]
    [Route("api/report/")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IImisModules _imisModules;

        public ReportController(IImisModules imisModules)
        {
            _imisModules = imisModules;
        }

        [HasRights(Rights.ReportsEnrolmentPerformanceIndicators)]
        [HttpPost]
        [Route("enrolment")]
        public IActionResult GetEnrolmentStats([FromBody]ReportRequestModel enrolmentRequestModel)
        {
            EnrolmentModel enrolmentModel;

            try
            {
                enrolmentModel = _imisModules.GetReportModule().GetReportLogic().GetEnrolmentStats(enrolmentRequestModel);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            if (enrolmentModel == null)
            {
                return NotFound();
            }

            return Ok(enrolmentModel);
        }

        [HasRights(Rights.ClaimFeedback)]
        [HttpPost]
        [Route("feedback")]
        public IActionResult GetFeedbackStats([FromBody]ReportRequestModel feedbackRequestModel)
        {
            FeedbackModel feedbackModel;

            try
            {
                feedbackModel = _imisModules.GetReportModule().GetReportLogic().GetFeedbackStats(feedbackRequestModel);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            if (feedbackModel == null)
            {
                return NotFound();
            }

            return Ok(feedbackModel);
        }

        [HasRights(Rights.ReportsRenewals)]
        [HttpPost]
        [Route("renewal")]
        public IActionResult GetRenewalStats([FromBody]ReportRequestModel renewalRequestModel)
        {
            RenewalModel renewalModel;

            try
            {
                renewalModel = _imisModules.GetReportModule().GetReportLogic().GetRenewalStats(renewalRequestModel);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            if (renewalModel == null)
            {
                return NotFound();
            }

            return Ok(renewalModel);
        }
    }
}