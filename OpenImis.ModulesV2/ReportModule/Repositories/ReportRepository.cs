using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.ReportModule.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace OpenImis.ModulesV2.ReportModule.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private IConfiguration _configuration;

        public ReportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EnrolmentModel GetEnrolmentStats(ReportRequestModel enrolmentRequestModel)
        {
            EnrolmentModel response = new EnrolmentModel();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var submitted = (from FP in imisContext.TblFromPhone
                                     join O in imisContext.TblOfficer on FP.OfficerCode equals O.Code
                                     where FP.DocType == "E"
                                     && FP.LandedDate >= DateTime.Parse(enrolmentRequestModel.FromDate)
                                     && FP.LandedDate <= DateTime.Parse(enrolmentRequestModel.ToDate)
                                     && O.ValidityTo == null
                                     && FP.OfficerCode == enrolmentRequestModel.OfficerCode
                                     select new { FromPhone = FP, Officer = O })
                                     .ToList();

                    var assigned = (from S in submitted
                                    from P in imisContext.TblPhotos
                                    where P.ValidityTo == null
                                    && P.PhotoFileName == S.FromPhone.DocName
                                    && P.OfficerId == S.Officer.OfficerId
                                    select S)
                                    .ToList();

                    response = new EnrolmentModel()
                    {
                        TotalSubmitted = submitted.Select(x => x.FromPhone).Count(),
                        TotalAssigned = assigned.Count(),
                    };
                }

                return response;
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public FeedbackModel GetFeedbackStats(ReportRequestModel feedbackRequestModel)
        {
            FeedbackModel response = new FeedbackModel();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var feedbackSent = (from FP in imisContext.TblFromPhone
                                        where FP.DocType == "F"
                                        && FP.LandedDate >= DateTime.Parse(feedbackRequestModel.FromDate)
                                        && FP.LandedDate <= DateTime.Parse(feedbackRequestModel.ToDate)
                                        && FP.OfficerCode == feedbackRequestModel.OfficerCode
                                        select FP)
                                        .ToList();

                    var feedbackAccepted = feedbackSent
                        .Where(f => f.DocStatus == "A")
                        .Count();

                    response = new FeedbackModel()
                    {
                        FeedbackSent = feedbackSent.Count(),
                        FeedbackAccepted = feedbackAccepted
                    };
                }

                return response;
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public RenewalModel GetRenewalStats(ReportRequestModel renewalRequestModel)
        {
            RenewalModel response = new RenewalModel();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var renewalSent = (from FP in imisContext.TblFromPhone
                                       where FP.DocType == "R"
                                       && FP.LandedDate >= DateTime.Parse(renewalRequestModel.FromDate)
                                       && FP.LandedDate <= DateTime.Parse(renewalRequestModel.ToDate)
                                       && FP.OfficerCode == renewalRequestModel.OfficerCode
                                       select FP)
                                      .ToList();

                    foreach (var item in renewalSent)
                    {
                        Debug.WriteLine(item.FromPhoneId);
                    }

                    var renewalAccepted = renewalSent
                        .Where(f => f.DocStatus == "A")
                        .Count();

                    response = new RenewalModel()
                    {
                        RenewalSent = renewalSent.Count(),
                        RenewalAccepted = renewalAccepted
                    };
                }

                return response;
            }
            catch (SqlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}