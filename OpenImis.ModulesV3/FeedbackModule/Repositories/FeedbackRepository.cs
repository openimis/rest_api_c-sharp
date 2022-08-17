using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.FeedbackModule.Models;
using OpenImis.ModulesV3.Helpers;
using OpenImis.ModulesV3.Utils;
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

namespace OpenImis.ModulesV3.FeedbackModule.Repositories
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

        // TODO Change the RV assignment codes. It should be on the list for better understanding
        public int Post(FeedbackRequest feedbackClaim)
        {
            int RV = 2;

            dynamic claimData;

            using (var imisContext = new ImisDB())
            {
                claimData = imisContext.TblClaim
                    .Where(u => u.ClaimUUID == feedbackClaim.ClaimUUID)
                    .Select(x => new { x.ClaimId, x.ClaimCode })
                    .FirstOrDefault();
            }

            string claimCode = claimData.ClaimCode.ToString();
            int claimId = int.Parse(claimData.ClaimId.ToString());

            Feedback feedback = new Feedback()
            {
                Officer = feedbackClaim.Officer,
                ClaimID = claimId,
                CHFID = feedbackClaim.CHFID,
                Answers = feedbackClaim.Answers,
                Date = feedbackClaim.Date
            };

            var XML = feedback.XMLSerialize();

            var tempDoc = new XmlDocument();
            tempDoc.LoadXml(XML);
            tempDoc.InnerXml = tempDoc.InnerXml.Replace("Feedback>", "feedback>");

            XML = tempDoc.OuterXml;

            var fromPhoneFeedbackDir = _configuration["AppSettings:FromPhone_Feedback"] + Path.DirectorySeparatorChar;
            var fromPhoneFeedbackRejectedDir = _configuration["AppSettings:FromPhone_Feedback_Rejected"] + Path.DirectorySeparatorChar;

            var fileName = "feedback_" + claimCode + ".xml";

            var xmldoc = new XmlDocument();
            xmldoc.InnerXml = XML;

            bool ifSaved = false;

            try
            {
                if (!Directory.Exists(fromPhoneFeedbackDir)) Directory.CreateDirectory(fromPhoneFeedbackDir);
                if (!Directory.Exists(fromPhoneFeedbackRejectedDir)) Directory.CreateDirectory(fromPhoneFeedbackRejectedDir);

                xmldoc.Save(fromPhoneFeedbackDir + fileName);
                ifSaved = true;
            }
            catch
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

                    using (DbCommand cmd = imisContext.CreateCommand())
                    {
                        cmd.CommandText = sql;

                        cmd.Parameters.AddRange(new[] { xmlParameter, returnParameter });

                        imisContext.CheckConnection();

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Displaying errors in the Stored Procedure in Debug mode
                            do
                            {
                                while (reader.Read())
                                {
                                    Debug.WriteLine("Error/Warning: " + reader.GetValue(0));
                                }
                            } while (reader.NextResult());
                        }
                    }

                    int tempRV = (int)returnParameter.Value;

                    if (tempRV == 0 || tempRV == 4)
                    {
                        RV = 1;
                    }
                    else if (tempRV == 1 || tempRV == 2 || tempRV == 3)
                    {
                        if (File.Exists(fromPhoneFeedbackDir + fileName))
                        {
                            File.Move(fromPhoneFeedbackDir + fileName, fromPhoneFeedbackRejectedDir + fileName);
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

        public List<FeedbackResponseModel> Get(string officerCode)
        {
            List<FeedbackResponseModel> response = new List<FeedbackResponseModel>();

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
                            select new FeedbackResponseModel()
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
                                DateFrom = C.DateFrom.ToString(DateTimeFormats.IsoDateFormat, CultureInfo.InvariantCulture),
                                DateTo = C.DateTo.Value.ToString(DateTimeFormats.IsoDateFormat, CultureInfo.InvariantCulture),
                                Phone = O.Phone,
                                FeedbackPromptDate = F.FeedbackPromptDate.ToString(DateTimeFormats.IsoDateFormat, CultureInfo.InvariantCulture)
                            })
                          .ToList();
            }

            return response;
        }
    }
}
