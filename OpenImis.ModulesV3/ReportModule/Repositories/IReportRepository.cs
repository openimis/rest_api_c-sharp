using OpenImis.ModulesV3.ReportModule.Models;

namespace OpenImis.ModulesV3.ReportModule.Repositories
{
    public interface IReportRepository
    {
        FeedbackReportModel GetFeedbackStats(ReportRequestModel feedbackRequestModel, string officerCode);
        RenewalReportModel GetRenewalStats(ReportRequestModel renewalRequestModel, string officerCode);
        EnrolmentReportModel GetEnrolmentStats(ReportRequestModel enrolmentRequestModel, string officerCode);
        SnapshotResponseModel GetSnapshotIndicators(SnapshotRequestModel snapshotRequestModel, string officerCode);
        CumulativeIndicatorsResponseModel GetCumulativeIndicators(IndicatorRequestModel cumulativeIndicatorsRequestModel, string officerCode);
    }
}
