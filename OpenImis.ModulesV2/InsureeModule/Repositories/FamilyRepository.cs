using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.Helpers;
using OpenImis.ModulesV2.InsureeModule.Models;
using OpenImis.ModulesV2.InsureeModule.Models.EnrollFamilyModels;
using System.Diagnostics;
using OpenImis.ModulesV2.InsureeModule.Helpers;
using System.Data.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Xml;

namespace OpenImis.ModulesV2.InsureeModule.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FamilyRepository(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public FamilyModel GetByCHFID(string chfid)
        {
            FamilyModel response = new FamilyModel();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = imisContext.TblInsuree
                                        .Where(i => i.ValidityTo == null && i.Chfid == chfid)
                                        .Join(imisContext.TblFamilies, i => i.FamilyId, f => f.FamilyId, (i, f) => f)
                                        .Where(f => f.ValidityTo == null)
                                        .Include(f => f.TblInsuree)
                                            .ThenInclude(f => f.Photo)
                                        .Select(f => FamilyModel.FromTblFamilies(f))
                                        .FirstOrDefault();
                }

                if (response == null)
                {
                    return null;
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

        public int Create(EnrollFamilyModel model, int userId, int officerId)
        {
            try
            {
                string webRootPath = _hostingEnvironment.WebRootPath;

                var XML = model.GetEnrolmentFromModel().Serialize();

                var EnrollmentDir = _configuration["AppSettings:Enrollment_Phone"];
                var JsonDebugFolder = _configuration["AppSettings:JsonDebugFolder"];
                var UpdatedFolder = _configuration["AppSettings:UpdatedFolder"];
                var SubmittedFolder = _configuration["AppSettings:SubmittedFolder"];

                var hof = "";

                if (model.Insuree.Any(x => x.isHead == "true" || x.isHead == "1"))
                {
                    hof = model.Insuree.Where(x => x.isHead == "true" || x.isHead == "1").Select(z => z.CHFID).FirstOrDefault();
                }
                else hof = "Unknown";

                var FileName = string.Format("{0}_{1}_{2}.xml", hof, officerId.ToString(), DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss.XML"));
                var JsonFileName = string.Format("{0}_{1}_{2}.xml", hof, officerId.ToString(), DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss"));


                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = XML;

                try
                {
                    if (!Directory.Exists(webRootPath + EnrollmentDir)) Directory.CreateDirectory(webRootPath + EnrollmentDir);

                    xmldoc.Save(webRootPath + EnrollmentDir + FileName);

                    if (!Directory.Exists(webRootPath + JsonDebugFolder)) Directory.CreateDirectory(webRootPath + JsonDebugFolder);

                    File.WriteAllText(webRootPath + JsonDebugFolder + JsonFileName, XML);
                }
                catch (Exception e)
                {
                    throw e;
                }

                int RV = -99;
                int InsureeUpd;
                int InsureeImported;

                using (var imisContext = new ImisDB())
                {
                    var xmlParameter = new SqlParameter("@XML", XML) { DbType = DbType.Xml };
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

                    var sql = "exec @RV = uspConsumeEnrollments @XML, @FamilySent OUT, @FamilyImported OUT, @FamiliesUpd OUT, @FamilyRejected OUT, " +
                        "@InsureeSent OUT, @InsureeUpd OUT, @InsureeImported OUT, " +
                        "@PolicySent OUT, @PolicyImported OUT, @PolicyRejected OUT, @PolicyChanged OUT," +
                        "@PremiumSent OUT, @PremiumImported OUT, @PremiumRejected OUT";

                    DbConnection connection = imisContext.Database.GetDbConnection();

                    using (DbCommand cmd = connection.CreateCommand())
                    {

                        cmd.CommandText = sql;

                        cmd.Parameters.AddRange(new[] { xmlParameter, returnParameter, familySentParameter, familyImportedParameter, familiesUpdParameter,
                                            familyRejectedParameter, insureeSentParameter, insureeUpdParameter, insureeImportedParameter, policySentParameter,
                                            policyImportedParameter, policyRejectedParameter, policyChangedParameter, premiumSentParameter, premiumImportedParameter,
                                            premiumRejectedParameter });

                        if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            //Displaying errors in the Stored Procedure in Debug mode
                            //do
                            //{
                            //    while (reader.Read())
                            //    {
                            //        Debug.WriteLine("Error/Warning: " + reader.GetValue(0));
                            //    }
                            //} while (reader.NextResult());
                        }
                    }

                    InsureeUpd = insureeUpdParameter.Value == DBNull.Value ? 0 : (int)insureeUpdParameter.Value;
                    InsureeImported = insureeImportedParameter.Value == DBNull.Value ? 0 : (int)insureeImportedParameter.Value;
                    RV = (int)returnParameter.Value;

                    if (RV == 0 && (InsureeImported > 0 || InsureeUpd > 0))
                    {
                        if (!Directory.Exists(webRootPath + UpdatedFolder))
                        {
                            Directory.CreateDirectory(webRootPath + UpdatedFolder);
                        }

                        foreach (var picture in model.Pictures)
                        {
                            if (picture != null)
                            {
                                if (picture.ImageContent != null)
                                {
                                    if (picture.ImageContent.Length == 0)
                                    {
                                        File.WriteAllBytes(webRootPath + UpdatedFolder + Path.DirectorySeparatorChar + picture.ImageName, picture.ImageContent);
                                    }
                                }
                            }
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

        public int GetUserIdByUUID(Guid uuid)
        {
            int response;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = imisContext.TblUsers
                        .Where(u => u.UserUUID == uuid)
                        .Select(x => x.UserId)
                        .FirstOrDefault();
                }

                return response;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public int GetOfficerIdByUserId(int userId)
        {
            int response;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var loginName = imisContext.TblUsers
                        .Where(u => u.UserId == userId)
                        .Select(x => x.LoginName)
                        .FirstOrDefault();

                    response = imisContext.TblOfficer
                        .Where(o => o.Code == loginName)
                        .Select(x => x.OfficerId)
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
