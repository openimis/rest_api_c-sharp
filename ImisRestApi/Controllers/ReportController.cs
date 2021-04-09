using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Modules;
using OpenImis.Modules.ReportModule.Models;
using OpenImis.Modules.Utils;
using ImisRestApi.Security;

namespace ImisRestApi.Controllers
{
    //[ApiVersion("2")]
    [Authorize]
    [Route("api/report/")]
    //[ApiController]
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
        public IActionResult GetEnrolmentStats([FromBody] ReportRequestModel enrolmentRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            EnrolmentReportModel enrolmentModel;

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

                Repository rep = new Repository();
                string officerCode = rep.GetLoginNameByUserUUID(userUUID);

                enrolmentModel = _imisModules.GetReportModule().GetReportLogic().GetEnrolmentStats(enrolmentRequestModel, officerCode);
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
        public IActionResult GetFeedbackStats([FromBody] ReportRequestModel feedbackRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            FeedbackReportModel feedbackModel;

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

                Repository rep = new Repository();
                string officerCode = rep.GetLoginNameByUserUUID(userUUID);

                feedbackModel = _imisModules.GetReportModule().GetReportLogic().GetFeedbackStats(feedbackRequestModel, officerCode);
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
        public IActionResult GetRenewalStats([FromBody] ReportRequestModel renewalRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            RenewalReportModel renewalModel;

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

                Repository rep = new Repository();
                string officerCode = rep.GetLoginNameByUserUUID(userUUID);

                renewalModel = _imisModules.GetReportModule().GetReportLogic().GetRenewalStats(renewalRequestModel, officerCode);
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

        [HasRights(Rights.ReportsPrimaryOperationalIndicatorPolicies)]
        [HttpPost]
        [Route("indicators/snapshot")]
        public IActionResult GetSnapshotIndicators([FromBody] SnapshotRequestModel snapshotRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            SnapshotResponseModel snapshotResponseModel;

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

                Repository rep = new Repository();
                string officerCode = rep.GetLoginNameByUserUUID(userUUID);

                snapshotResponseModel = _imisModules.GetReportModule().GetReportLogic().GetSnapshotIndicators(snapshotRequestModel, officerCode);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            if (snapshotResponseModel == null)
            {
                return NotFound();
            }

            return Ok(snapshotResponseModel);
        }

        [HasRights(Rights.ReportsPrimaryOperationalIndicatorPolicies)]
        [HttpPost]
        [Route("indicators/cumulative")]
        public IActionResult GetCumulativeIndicators([FromBody] IndicatorRequestModel cumulativeRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            CumulativeIndicatorsResponseModel cumulativeIndicatorsResponseModel;

            try
            {
                Guid userUUID = Guid.Parse(HttpContext.User.Claims.Where(w => w.Type == "UserUUID").Select(x => x.Value).FirstOrDefault());

                Repository rep = new Repository();
                string officerCode = rep.GetLoginNameByUserUUID(userUUID);

                cumulativeIndicatorsResponseModel = _imisModules.GetReportModule().GetReportLogic().GetCumulativeIndicators(cumulativeRequestModel, officerCode);
            }
            catch (ValidationException e)
            {
                return BadRequest(new { error = new { message = e.Message, value = e.Value } });
            }

            if (cumulativeIndicatorsResponseModel == null)
            {
                return NotFound();
            }

            return Ok(cumulativeIndicatorsResponseModel);
        }
    }
}
