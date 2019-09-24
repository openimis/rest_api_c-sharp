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

        public EnrolmentReportModel GetEnrolmentStats(ReportRequestModel enrolmentRequestModel, string officerCode)
        {
            EnrolmentReportModel response = new EnrolmentReportModel();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var submitted = (from FP in imisContext.TblFromPhone
                                     join O in imisContext.TblOfficer on FP.OfficerCode equals O.Code
                                     where FP.DocType == "E"
                                     && FP.LandedDate >= enrolmentRequestModel.FromDate
                                     && FP.LandedDate <= enrolmentRequestModel.ToDate
                                     && O.ValidityTo == null
                                     && FP.OfficerCode == officerCode
                                     select new { FromPhone = FP, Officer = O })
                                     .ToList();

                    var assigned = (from S in submitted
                                    from P in imisContext.TblPhotos
                                    where P.ValidityTo == null
                                    && P.PhotoFileName == S.FromPhone.DocName
                                    && P.OfficerId == S.Officer.OfficerId
                                    select S)
                                    .ToList();

                    response = new EnrolmentReportModel()
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

        public FeedbackReportModel GetFeedbackStats(ReportRequestModel feedbackRequestModel, string officerCode)
        {
            FeedbackReportModel response = new FeedbackReportModel();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var feedbackSent = (from FP in imisContext.TblFromPhone
                                        where FP.DocType == "F"
                                        && FP.LandedDate >= feedbackRequestModel.FromDate
                                        && FP.LandedDate <= feedbackRequestModel.ToDate
                                        && FP.OfficerCode == officerCode
                                        select FP)
                                        .ToList();

                    var feedbackAccepted = feedbackSent
                        .Where(f => f.DocStatus == "A")
                        .Count();

                    response = new FeedbackReportModel()
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

        public RenewalReportModel GetRenewalStats(ReportRequestModel renewalRequestModel, string officerCode)
        {
            RenewalReportModel response = new RenewalReportModel();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var renewalSent = (from FP in imisContext.TblFromPhone
                                       where FP.DocType == "R"
                                       && FP.LandedDate >= renewalRequestModel.FromDate
                                       && FP.LandedDate <= renewalRequestModel.ToDate
                                       && FP.OfficerCode == officerCode
                                       select FP)
                                      .ToList();

                    foreach (var item in renewalSent)
                    {
                        Debug.WriteLine(item.FromPhoneId);
                    }

                    var renewalAccepted = renewalSent
                        .Where(f => f.DocStatus == "A")
                        .Count();

                    response = new RenewalReportModel()
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

        public string GetLoginNameByUserUUID(Guid userUUID)
        {
            string response;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = imisContext.TblUsers
                        .Where(u => u.UserUUID == userUUID)
                        .Select(x => x.LoginName)
                        .FirstOrDefault();
                }

                return response;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}