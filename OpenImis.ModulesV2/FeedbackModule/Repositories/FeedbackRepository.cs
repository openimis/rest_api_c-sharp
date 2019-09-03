using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.FeedbackModule.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenImis.ModulesV2.FeedbackModule.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private IConfiguration _configuration;

        public FeedbackRepository(IConfiguration configuration)
        {
            _configuration = configuration;
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
