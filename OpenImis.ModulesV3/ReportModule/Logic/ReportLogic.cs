using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV3.ReportModule.Models;
using OpenImis.ModulesV3.ReportModule.Repositories;
using System;

namespace OpenImis.ModulesV3.ReportModule.Logic
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

        public FeedbackReportModel GetFeedbackStats(ReportRequestModel feedbackRequestModel, string officerCode)
        {
            FeedbackReportModel response;

            response = reportRepository.GetFeedbackStats(feedbackRequestModel, officerCode);

            return response;
        }

        public RenewalReportModel GetRenewalStats(ReportRequestModel renewalRequestModel, string officerCode)
        {
            RenewalReportModel response;

            response = reportRepository.GetRenewalStats(renewalRequestModel, officerCode);

            return response;
        }

        public EnrolmentReportModel GetEnrolmentStats(ReportRequestModel enrolmentRequestModel, string officerCode)
        {
            EnrolmentReportModel response;

            response = reportRepository.GetEnrolmentStats(enrolmentRequestModel, officerCode);

            return response;
        }

        public SnapshotResponseModel GetSnapshotIndicators(SnapshotRequestModel snapshotRequestModel, string officerCode)
        {
            SnapshotResponseModel response;

            response = reportRepository.GetSnapshotIndicators(snapshotRequestModel, officerCode);

            return response;
        }

        public CumulativeIndicatorsResponseModel GetCumulativeIndicators(IndicatorRequestModel cumulativeIndicatorsRequestModel, string officerCode)
        {
            CumulativeIndicatorsResponseModel response;

            response = reportRepository.GetCumulativeIndicators(cumulativeIndicatorsRequestModel, officerCode);

            return response;
        }
    }
}
