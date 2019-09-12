using OpenImis.ModulesV2.ReportModule.Models;

namespace OpenImis.ModulesV2.ReportModule.Logic
{
    public interface IReportLogic
    {
        FeedbackModel GetFeedbackStats(ReportRequestModel feedbackRequestModel);
        RenewalModel GetRenewalStats(ReportRequestModel renewalRequestModel);
        EnrolmentModel GetEnrolmentStats(ReportRequestModel enrolmentRequestModel);
    }
}
