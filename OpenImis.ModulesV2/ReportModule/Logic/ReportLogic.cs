using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.ReportModule.Models;
using OpenImis.ModulesV2.ReportModule.Repositories;

namespace OpenImis.ModulesV2.ReportModule.Logic
{
    public class ReportLogic : IReportLogic
    {
        private IConfiguration _configuration;
        protected IReportRepository reportRepository;

        public ReportLogic(IConfiguration configuration)
        {
            _configuration = configuration;
            reportRepository = new ReportRepository(_configuration);
        }

        public FeedbackModel GetFeedbackStats(ReportRequestModel feedbackRequestModel)
        {
            FeedbackModel response;

            response = reportRepository.GetFeedbackStats(feedbackRequestModel);

            return response;
        }

        public RenewalModel GetRenewalStats(ReportRequestModel renewalRequestModel)
        {
            RenewalModel response;

            response = reportRepository.GetRenewalStats(renewalRequestModel);

            return response;
        }

        public EnrolmentModel GetEnrolmentStats(ReportRequestModel enrolmentRequestModel)
        {
            EnrolmentModel response;

            response = reportRepository.GetEnrolmentStats(enrolmentRequestModel);

            return response;
        }
    }
}
