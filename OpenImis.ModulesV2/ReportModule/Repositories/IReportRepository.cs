using OpenImis.ModulesV2.ReportModule.Models;

namespace OpenImis.ModulesV2.ReportModule.Repositories
{
    public interface IReportRepository
    {
        FeedbackModel GetFeedbackStats(ReportRequestModel feedbackRequestModel);
        RenewalModel GetRenewalStats(ReportRequestModel renewalRequestModel);
        EnrolmentModel GetEnrolmentStats(ReportRequestModel enrolmentRequestModel);
    }
}
