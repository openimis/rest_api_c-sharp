using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.ReportModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace OpenImis.ModulesV3.ReportModule.Repositories
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

        public FeedbackReportModel GetFeedbackStats(ReportRequestModel feedbackRequestModel, string officerCode)
        {
            FeedbackReportModel response = new FeedbackReportModel();

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

        public RenewalReportModel GetRenewalStats(ReportRequestModel renewalRequestModel, string officerCode)
        {
            RenewalReportModel response = new RenewalReportModel();

            using (var imisContext = new ImisDB())
            {
                var renewalSent = (from FP in imisContext.TblFromPhone
                                   where FP.DocType == "R"
                                   && FP.LandedDate >= renewalRequestModel.FromDate
                                   && FP.LandedDate <= renewalRequestModel.ToDate
                                   && FP.OfficerCode == officerCode
                                   select FP)
                                  .ToList();

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

        public SnapshotResponseModel GetSnapshotIndicators(SnapshotRequestModel snapshotRequestModel, string officerCode)
        {
            SnapshotResponseModel response = new SnapshotResponseModel();

            int officerId;

            using (var imisContext = new ImisDB())
            {
                officerId = (from O in imisContext.TblOfficer
                             where O.Code == officerCode
                             && O.ValidityTo == null
                             select O.OfficerId)
                             .FirstOrDefault();

                var snapshotDateParameter = new SqlParameter("@SnapshotDate", snapshotRequestModel.SnapshotDate) { SqlDbType = SqlDbType.Date };
                var officerIdParameter = new SqlParameter("@OfficerId", officerId);

                var sql = "SELECT Active, Expired, Idle, Suspended FROM udfGetSnapshotIndicators(@SnapshotDate,@OfficerId)";

                using (DbCommand cmd = imisContext.CreateCommand())
                {
                    cmd.CommandText = sql;

                    cmd.Parameters.AddRange(new[] { snapshotDateParameter, officerIdParameter });

                    imisContext.CheckConnection();

                    using (var reader = cmd.ExecuteReader())
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                response.Active = int.Parse(reader["Active"].ToString());
                                response.Expired = int.Parse(reader["Expired"].ToString());
                                response.Idle = int.Parse(reader["Idle"].ToString());
                                response.Suspended = int.Parse(reader["Suspended"].ToString());
                            }
                        } while (reader.NextResult());
                    }
                }
            }

            return response;
        }

        public CumulativeIndicatorsResponseModel GetCumulativeIndicators(IndicatorRequestModel cumulativeIndicatorsRequestModel, string officerCode)
        {
            CumulativeIndicatorsResponseModel response = new CumulativeIndicatorsResponseModel();

            int officerId;

            using (var imisContext = new ImisDB())
            {
                officerId = (from O in imisContext.TblOfficer
                             where O.Code == officerCode
                             && O.ValidityTo == null
                             select O.OfficerId)
                             .FirstOrDefault();

                var dateFromParameter = new SqlParameter("@DateFrom", cumulativeIndicatorsRequestModel.FromDate) { SqlDbType = SqlDbType.Date };
                var dateToParameter = new SqlParameter("@DateTo", cumulativeIndicatorsRequestModel.ToDate) { SqlDbType = SqlDbType.Date };
                var officerIdParameter = new SqlParameter("@OfficerId", officerId);

                var sql = "SELECT " +
                    " ISNULL(dbo.udfNewPoliciesPhoneStatistics(@DateFrom,@DateTo,@OfficerId),0) NewPolicies," +
                    " ISNULL(dbo.udfRenewedPoliciesPhoneStatistics(@DateFrom,@DateTo,@OfficerId),0) RenewedPolicies, " +
                    " ISNULL(dbo.udfExpiredPoliciesPhoneStatistics(@DateFrom,@DateTo,@OfficerId),0) ExpiredPolicies,  " +
                    " ISNULL(dbo.udfSuspendedPoliciesPhoneStatistics(@DateFrom,@DateTo,@OfficerId),0) SuspendedPolicies," +
                    " ISNULL(dbo.udfCollectedContribution(@DateFrom,@DateTo,@OfficerId),0) CollectedContribution ";

                DbConnection connection = imisContext.Database.GetDbConnection();

                using (DbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;

                    cmd.Parameters.AddRange(new[] { dateFromParameter, dateToParameter, officerIdParameter });

                    if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                response.NewPolicies = int.Parse(reader["NewPolicies"].ToString());
                                response.RenewedPolicies = int.Parse(reader["RenewedPolicies"].ToString());
                                response.ExpiredPolicies = int.Parse(reader["ExpiredPolicies"].ToString());
                                response.SuspendedPolicies = int.Parse(reader["SuspendedPolicies"].ToString());
                                response.CollectedContribution = Math.Round(double.Parse(reader["CollectedContribution"].ToString()), 2);
                            }
                        } while (reader.NextResult());
                    }
                }
            }

            return response;
        }
    }
}