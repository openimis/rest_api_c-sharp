using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.Helpers;
using OpenImis.ModulesV3.InsureeModule.Models;
using OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels;
using System.Data.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Xml;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using OpenImis.ModulesV3.InsureeModule.Repositories;
using OpenImis.DB.SqlServer.DataHelper;
using OpenImis.ePayment.Logic;
using OpenImis.ePayment.Data;
using OpenImis.ModulesV3.Utils;
using OpenImis.ModulesV3.InsureeModule.Logic;
using Microsoft.Extensions.Logging;

namespace OpenImis.ModulesV3.InsureeModule.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;

        public FamilyRepository(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<FamilyRepository>();
        }

        public FamilyModel GetByCHFID(string chfid, Guid userUUID)
        {
            FamilyModel response = new FamilyModel();

            using (var imisContext = new ImisDB())
            {
                var locationIds = (from UD in imisContext.TblUsersDistricts
                                   join U in imisContext.TblUsers on UD.UserId equals U.UserId
                                   where U.UserUUID == userUUID && U.ValidityTo == null && UD.ValidityTo == null
                                   select UD.LocationId)
                                   .ToList();

                var familyId = (from I in imisContext.TblInsuree
                                join F in imisContext.TblFamilies on I.FamilyId equals F.FamilyId
                                join LV in imisContext.TblLocations on F.LocationId equals LV.LocationId
                                join LW in imisContext.TblLocations on LV.ParentLocationId equals LW.LocationId
                                join LD in imisContext.TblLocations on LW.ParentLocationId equals LD.LocationId
                                where (I.Chfid == chfid
                                    && I.ValidityTo == null
                                    && F.ValidityTo == null
                                    && LV.ValidityTo == null
                                    && LW.ValidityTo == null
                                    && LD.ValidityTo == null
                                    && locationIds.Contains(LD.LocationId))
                                select F.FamilyId)
                               .FirstOrDefault();

                response = imisContext.TblInsuree
                                    .Where(i => i.ValidityTo == null && i.FamilyId == familyId)
                                    .Join(imisContext.TblFamilies, i => i.FamilyId, f => f.FamilyId, (i, f) => f)
                                    .Where(f => f.ValidityTo == null)
                                    .Include(f => f.TblInsuree)
                                        .ThenInclude(f => f.Photo)
                                    .Include(f => f.TblFamilySMS)
                                    .Select(f => FamilyModel.FromTblFamilies(f))
                                    .FirstOrDefault();
            }

            return response;
        }

        public NewFamilyResponse CreateEnrolResponse(EnrolFamilyModel model)
        {
            var response = new NewFamilyResponse();

            using (var imisContext = new ImisDB())
            {
                foreach (var fam in model.Family)
                {

                    var familyId = imisContext.TblInsuree.Where(i => i.Chfid == fam.HOFCHFID && i.IsHead == true && i.ValidityTo == null)
                                    .Select(i => i.FamilyId)
                                    .FirstOrDefault();

                    var family = imisContext.TblFamilies
                                .Where(f => f.FamilyId == familyId && f.ValidityTo == null)
                                .Include(f => f.TblInsuree)
                                .Include(p => p.TblPolicy)
                                    .ThenInclude(pr => pr.TblPremium)
                                .FirstOrDefault();

                    // if the family is not found means it failed to insert
                    if (family == null)
                        continue;

                    family.TblInsuree = family.TblInsuree.Where(i => i.ValidityTo == null).ToList();
                    family.TblPolicy = family.TblPolicy.Where(p => p.ValidityTo == null).ToList();


                    var familyVM = new FamilyVM
                    {
                        FamilyId = fam.FamilyId,
                        FamilyDBId = family.FamilyId,
                        FamilyUUID = family.FamilyUUID
                    };

                    foreach (var i in family.TblInsuree)
                    {
                        familyVM.Insurees.Add(new InsureeVM
                        {
                            InsureeId = fam.Insurees.Where(x => x.CHFID == i.Chfid).Select(x => x.InsureeId).FirstOrDefault(),
                            InsureeDBId = i.InsureeId,
                            InsureeUUID = i.InsureeUUID
                        });
                    }

                    foreach (var p in family.TblPolicy)
                    {
                        var premiums = new List<PremiumVM>();
                        p.TblPremium = p.TblPremium.Where(prem => prem.ValidityTo == null).ToList();
                        foreach (var pr in p.TblPremium)
                        {
                            premiums.Add(new PremiumVM
                            {
                                PremiumId = fam.Policies.Count > 0 ? fam.Policies.Where(x => x.ProdId == p.ProdId).FirstOrDefault().Premium.Where(y => y.Receipt == pr.Receipt).Select(z => z.PremiumId).FirstOrDefault() : 0,
                                PremiumDBId = pr.PremiumId,
                                PremiumUUID = pr.PremiumUUID
                            });
                        }
                        familyVM.Policies.Add(new PolicyVM
                        {
                            PolicyId = fam.Policies.Where(x => x.ProdId == p.ProdId).Select(y => y.PolicyId).FirstOrDefault(),
                            PolicyDBId = p.PolicyId,
                            PolicyUUID = p.PolicyUUID,
                            Premium = premiums
                        });

                    }

                    response.Family.Add(familyVM);
                }
            }
            return response;
        }

        public NewFamilyResponse Create(EnrolFamilyModel model, int userId, int officerId)
        {
            var enrolFamily = model.GetEnrolmentFromModel();

            enrolFamily.FileInfo.UserId = userId;
            enrolFamily.FileInfo.OfficerId = officerId;

            var XML = enrolFamily.XMLSerialize();
            var JSON = JsonConvert.SerializeObject(enrolFamily);

            var dateFolder = DateTime.Now.Year.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Month.ToString() + Path.DirectorySeparatorChar + DateTime.Now.Day.ToString() + Path.DirectorySeparatorChar;

            var EnrolmentDir = _configuration["AppSettings:Enrollment_Phone"] + Path.DirectorySeparatorChar + dateFolder;

            var UpdatedFolder = _configuration["AppSettings:UpdatedFolder"] + Path.DirectorySeparatorChar;
            var SubmittedFolder = _configuration["AppSettings:SubmittedFolder"] + Path.DirectorySeparatorChar;


            var hof = enrolFamily.Families.Select(x => x.HOFCHFID).FirstOrDefault();

            var FileName = string.Format("{0}_{1}_{2}.xml", hof, officerId.ToString(), DateTime.Now.ToString(DateTimeFormats.FileNameDateTimeFormat));


            var xmldoc = new XmlDocument();
            xmldoc.InnerXml = XML;

            if (!Directory.Exists(EnrolmentDir)) Directory.CreateDirectory(EnrolmentDir);

            xmldoc.Save(EnrolmentDir + FileName);



            int RV = -99;
            int InsureeUpd;
            int InsureeImported;

            using (var imisContext = new ImisDB())
            {
                var xmlParameter = new SqlParameter("@XML", XML) { DbType = DbType.Xml };
                var source = new SqlParameter("@Source", model.Source) { DbType = DbType.String, Size = 50 };
                var sourceVersion = new SqlParameter("@SourceVersion", model.SourceVersion) { DbType = DbType.String, Size = 15 };
                var returnParameter = OutputParameter.CreateOutputParameter("@RV", SqlDbType.Int);
                var familySentParameter = OutputParameter.CreateOutputParameter("@FamilySent", SqlDbType.Int);
                var familyImportedParameter = OutputParameter.CreateOutputParameter("@FamilyImported", SqlDbType.Int);
                var familiesUpdParameter = OutputParameter.CreateOutputParameter("@FamiliesUpd", SqlDbType.Int);
                var familyRejectedParameter = OutputParameter.CreateOutputParameter("@FamilyRejected", SqlDbType.Int);
                var insureeSentParameter = OutputParameter.CreateOutputParameter("@InsureeSent", SqlDbType.Int);
                var insureeUpdParameter = OutputParameter.CreateOutputParameter("@InsureeUpd", SqlDbType.Int);
                var insureeImportedParameter = OutputParameter.CreateOutputParameter("@InsureeImported", SqlDbType.Int);
                var policySentParameter = OutputParameter.CreateOutputParameter("@PolicySent", SqlDbType.Int);
                var policyImportedParameter = OutputParameter.CreateOutputParameter("@PolicyImported", SqlDbType.Int);
                var policyRejectedParameter = OutputParameter.CreateOutputParameter("@PolicyRejected", SqlDbType.Int);
                var policyChangedParameter = OutputParameter.CreateOutputParameter("@PolicyChanged", SqlDbType.Int);
                var premiumSentParameter = OutputParameter.CreateOutputParameter("@PremiumSent", SqlDbType.Int);
                var premiumImportedParameter = OutputParameter.CreateOutputParameter("@PremiumImported", SqlDbType.Int);
                var premiumRejectedParameter = OutputParameter.CreateOutputParameter("@PremiumRejected", SqlDbType.Int);

                var sql = "exec @RV = uspConsumeEnrollments @XML, @Source, @SourceVersion, @FamilySent OUT, @FamilyImported OUT, @FamiliesUpd OUT, @FamilyRejected OUT, " +
                    "@InsureeSent OUT, @InsureeUpd OUT, @InsureeImported OUT, " +
                    "@PolicySent OUT, @PolicyImported OUT, @PolicyRejected OUT, @PolicyChanged OUT," +
                    "@PremiumSent OUT, @PremiumImported OUT, @PremiumRejected OUT";

                using (DbCommand cmd = imisContext.CreateCommand())
                {

                    cmd.CommandText = sql;

                    cmd.Parameters.AddRange(new[] { xmlParameter, source, sourceVersion, returnParameter, familySentParameter, familyImportedParameter, familiesUpdParameter,
                                            familyRejectedParameter, insureeSentParameter, insureeUpdParameter, insureeImportedParameter, policySentParameter,
                                            policyImportedParameter, policyRejectedParameter, policyChangedParameter, premiumSentParameter, premiumImportedParameter,
                                            premiumRejectedParameter });

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

                InsureeUpd = insureeUpdParameter.Value == DBNull.Value ? 0 : (int)insureeUpdParameter.Value;
                InsureeImported = insureeImportedParameter.Value == DBNull.Value ? 0 : (int)insureeImportedParameter.Value;
                RV = (int)returnParameter.Value;

                if (RV == 0 && (InsureeImported > 0 || InsureeUpd > 0))
                {
                    if (!Directory.Exists(UpdatedFolder))
                    {
                        Directory.CreateDirectory(UpdatedFolder);
                    }

                    foreach (var picture in model.Family.Select(x => x.Insurees.Select(s => s.Picture)).FirstOrDefault().ToList())
                    {
                        if (picture != null)
                        {
                            if (picture.ImageContent != null)
                            {
                                if (picture.ImageContent.Length != 0)
                                {
                                    File.WriteAllBytes(UpdatedFolder + Path.DirectorySeparatorChar + picture.ImageName, Convert.FromBase64String(picture.ImageContent));
                                }
                            }
                        }
                    }
                }
            }

            var newFamily = new NewFamilyResponse();
            newFamily.Response = RV;
            if (RV == 0)
            {
                newFamily = CreateEnrolResponse(model);

                // Update the control number
                newFamily.Response = UpdateControlNumber(model, newFamily);

                // Create Premium
                CreatePremium(model);
            }

            return newFamily;
        }

        public int GetUserIdByUUID(Guid uuid)
        {
            int response;

            using (var imisContext = new ImisDB())
            {
                response = imisContext.TblUsers
                    .Where(u => u.UserUUID == uuid)
                    .Select(x => x.UserId)
                    .FirstOrDefault();
            }

            return response;

        }

        public int GetOfficerIdByUserUUID(Guid userUUID)
        {
            int response;

            using (var imisContext = new ImisDB())
            {
                var loginName = imisContext.TblUsers
                    .Where(u => u.UserUUID == userUUID)
                    .Select(x => x.LoginName)
                    .FirstOrDefault();

                response = imisContext.TblOfficer
                    .Where(o => o.Code == loginName)
                    .Select(x => x.OfficerId)
                    .FirstOrDefault();
            }

            return response;
        }

        public int UpdateControlNumber(EnrolFamilyModel familyModel, NewFamilyResponse serverResponse)
        {

            foreach (var family in familyModel.Family)
            {
                if (family.Policies != null)
                {
                    foreach (var policy in family.Policies)
                    {
                        if (String.IsNullOrEmpty(policy.ControlNumber))
                            continue;

                        var policyId = serverResponse.Family.Where(f => f.FamilyId == family.FamilyId).FirstOrDefault().Policies.Where(p => p.PolicyId == policy.PolicyId).Select(p => p.PolicyDBId).FirstOrDefault();

                        var sSQL = @"UPDATE PD SET InsuranceNumber = I.CHFID, PremiumID = PR.PremiumId, PolicyStage = Pol.PolicyStage, enrollmentDate = Pol.EnrollDate, ValidityFrom = GETDATE()
                            FROM tblControlNumber CN
                            INNER JOIN tblPaymentDetails PD ON CN.PaymentId = PD.PaymentID
                            INNER JOIN tblInsuree I ON IsHead = 1 
                            INNER JOIN tblPolicy Pol ON Pol.PolicyID = @PolicyId AND Pol.FamilyID = I.FamilyId
                            LEFT OUTER JOIN tblPremium PR ON Pol.PolicyID = PR.PolicyID
                            WHERE CN.ValidityTo IS NULL
                            AND I.ValidityTo IS NULL
                            AND CN.ControlNumber = @ControlNumber;";

                        SqlParameter[] parameters =
                        {
                            new SqlParameter("@ControlNumber", policy.ControlNumber),
                            new SqlParameter("@PolicyId", policyId),
                        };

                        try
                        {
                            var dh = new DB.SqlServer.DataHelper.DataHelper(_configuration);
                            dh.Execute(sSQL, parameters, CommandType.Text);
                        }
                        catch (Exception)
                        {
                            return 1001;
                        }

                    }
                }
                else
                {
                    _logger.LogWarning("Family policy null in UpdateControlNumber, family HOFCHFID: " + family.HOFCHFID);
                }
            }
            return 0;
        }

        public void CreatePremium(EnrolFamilyModel model)
        {
            ImisPayment payment = new ImisPayment(_configuration, _hostingEnvironment, _loggerFactory);
            PaymentLogic paymentLogic = new PaymentLogic(_configuration, _hostingEnvironment, _loggerFactory);

            if (_configuration.GetValue<bool>("PaymentGateWay:CreatePremiumOnPaymentReceived"))
            {
                foreach (var family in model.Family)
                {
                    foreach (var policy in family.Policies)
                    {
                        int paymentId = payment.GetPaymentId(policy.ControlNumber);
                        _ = paymentLogic.CreatePremium(paymentId, "GePG", model.SourceVersion);
                    }
                }
            }
        }
    }
}