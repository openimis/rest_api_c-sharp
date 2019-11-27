using OpenImis.ModulesV2.ReportModule.Models;

namespace OpenImis.ModulesV2.ReportModule.Repositories
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
