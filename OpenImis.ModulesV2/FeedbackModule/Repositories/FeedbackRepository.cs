using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.FeedbackModule.Models;
using OpenImis.ModulesV2.Helpers;
using OpenImis.ModulesV2.InsureeModule.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace OpenImis.ModulesV2.FeedbackModule.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FeedbackRepository(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public int Post(Feedback feedbackClaim)
        {
            int RV = 2;

            try
            {
                string webRootPath = _hostingEnvironment.WebRootPath;

                var XML = feedbackClaim.XMLSerialize();

                var tempDoc = new XmlDocument();
                tempDoc.LoadXml(XML);
                tempDoc.InnerXml = tempDoc.InnerXml.Replace("Feedback>", "feedback>");

                XML = tempDoc.OuterXml;

                var fromPhoneFeedbackDir = _configuration["AppSettings:FromPhone_Feedback"];
                var fromPhoneFeedbackRejectedDir = _configuration["AppSettings:FromPhone_Feedback_Rejected"];

                var claimCode = "";

                using (var imisContext = new ImisDB())
                {
                    claimCode = imisContext.TblClaim
                        .Where(u => u.ClaimId == feedbackClaim.ClaimID)
                        .Select(x => x.ClaimCode)
                        .FirstOrDefault();
                }

                var fileName = "feedback_" + claimCode + ".xml";

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = XML;

                bool ifSaved = false;

                try
                {
                    if (!Directory.Exists(webRootPath + fromPhoneFeedbackDir)) Directory.CreateDirectory(webRootPath + fromPhoneFeedbackDir);

                    xmldoc.Save(webRootPath + fromPhoneFeedbackDir + fileName);
                    ifSaved = true;
                }
                catch (Exception e)
                {
                    return RV;
                }

                if (ifSaved)
                {
                    using (var imisContext = new ImisDB())
                    {
                        var xmlParameter = new SqlParameter("@XML", XML) { DbType = DbType.Xml };
                        var returnParameter = OutputParameter.CreateOutputParameter("@RV", SqlDbType.Int);

                        var sql = "exec @RV = uspInsertFeedback @XML";

                        DbConnection connection = imisContext.Database.GetDbConnection();

                        using (DbCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = sql;

                            cmd.Parameters.AddRange(new[] { xmlParameter, returnParameter });

                            if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                            using (var reader = cmd.ExecuteReader())
                            {
                                // Displaying errors in the Stored Procedure in Debug mode
                                //do
                                //{
                                //    while (reader.Read())
                                //    {
                                //        Debug.WriteLine("Error/Warning: " + reader.GetValue(0));
                                //    }
                                //} while (reader.NextResult());
                            }
                        }

                        int tempRV = (int)returnParameter.Value;

                        if (tempRV == 0 || tempRV == 4)
                        {
                            RV = 1;
                        }
                        else if (tempRV == 1 || tempRV == 2 || tempRV == 3)
                        {
                            if (File.Exists(webRootPath + fromPhoneFeedbackDir + fileName))
                            {
                                File.Move(webRootPath + fromPhoneFeedbackDir + fileName, webRootPath + fromPhoneFeedbackRejectedDir);
                            }
                            RV = 0;
                        }
                        else
                        {
                            RV = 2;
                        }
                    }
                }

                return RV;
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

        public List<FeedbackModel> Get(string officerCode)
        {
            List<FeedbackModel> response = new List<FeedbackModel>();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = (from F in imisContext.TblFeedbackPrompt
                                join O in imisContext.TblOfficer on F.OfficerId equals O.OfficerId
                                join C in imisContext.TblClaim on F.ClaimId equals C.ClaimId
                                join I in imisContext.TblInsuree on C.InsureeId equals I.InsureeId
                                join HF in imisContext.TblHf on C.Hfid equals HF.HfId
                                where F.ValidityTo == null
                                && O.ValidityTo == null
                                && O.Code == officerCode
                                && C.FeedbackStatus == 4
                                select new FeedbackModel()
                                {
                                    ClaimUUID = C.ClaimUUID,
                                    OfficerId = F.OfficerId,
                                    OfficerCode = O.Code,
                                    CHFID = I.Chfid,
                                    LastName = I.LastName,
                                    OtherNames = I.OtherNames,
                                    HFCode = HF.Hfcode,
                                    HFName = HF.Hfname,
                                    ClaimCode = C.ClaimCode,
                                    DateFrom = C.DateFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    DateTo = C.DateTo.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    Phone = O.Phone,
                                    FeedbackPromptDate = F.FeedbackPromptDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                                })
                              .ToList();
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
