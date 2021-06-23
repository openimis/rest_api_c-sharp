using OpenImis.ModulesV3.ReportModule.Models;
using System;

namespace OpenImis.ModulesV3.ReportModule.Logic
{
    public interface IReportLogic
    {
        FeedbackReportModel GetFeedbackStats(ReportRequestModel feedbackRequestModel, string officerCode);
        RenewalReportModel GetRenewalStats(ReportRequestModel renewalRequestModel, string officerCode);
        EnrolmentReportModel GetEnrolmentStats(ReportRequestModel enrolmentRequestModel, string officerCode);
        SnapshotResponseModel GetSnapshotIndicators(SnapshotRequestModel snapshotRequestModel, string officerCode);
        CumulativeIndicatorsResponseModel GetCumulativeIndicators(IndicatorRequestModel cumulativeIndicatorsRequestModel, string officerCode);
    }
}
