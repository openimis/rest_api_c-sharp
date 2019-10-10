using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.ClaimModule.Models.RegisterClaim;
using OpenImis.ModulesV2.Helpers;

namespace OpenImis.ModulesV2.ClaimModule.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ClaimRepository(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // TODO Change the RV assignment codes. It should be on the list for better understanding
        public int Create(Claim claim)
        {
            try
            {
                var XML = claim.XMLSerialize();
                var RV = 2;

                bool ifSaved = false;

                string webRootPath = _hostingEnvironment.WebRootPath;

                var fromPhoneClaimDir = _configuration["AppSettings:FromPhone_Claim"];
                var fromPhoneClaimRejectedDir = _configuration["AppSettings:FromPhone_Claim_Rejected"];

                var fileName = "Claim_" + claim.Details.HFCode + "_" + claim.Details.CHFID + "_" + claim.Details.ClaimCode + ".xml";

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = XML;

                try
                {

                    if (!Directory.Exists(webRootPath + fromPhoneClaimDir)) Directory.CreateDirectory(webRootPath + fromPhoneClaimDir);
                    if (!Directory.Exists(webRootPath + fromPhoneClaimRejectedDir)) Directory.CreateDirectory(webRootPath + fromPhoneClaimRejectedDir);

                    if (!File.Exists(webRootPath + fromPhoneClaimDir + fileName))
                    {
                        xmldoc.Save(webRootPath + fromPhoneClaimDir + fileName);
                    }

                    ifSaved = true;
                }
                catch (Exception e)
                {
                    return 2;
                }

                if (ifSaved)
                {
                    using (var imisContext = new ImisDB())
                    {
                        var xmlParameter = new SqlParameter("@XML", XML) { DbType = DbType.Xml };
                        var returnParameter = new SqlParameter("@RV", SqlDbType.Int) { Direction = ParameterDirection.Output };

                        var sql = "exec @RV = uspUpdateClaimFromPhone @XML";

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

                        if (tempRV == 0)
                        {
                            RV = 1;
                        }
                        else if (tempRV == -1)
                        {
                            RV = 2;
                        }
                        else
                        {
                            if (File.Exists(webRootPath + fromPhoneClaimDir + fileName) && !File.Exists(webRootPath + fromPhoneClaimRejectedDir + fileName))
                            {
                                File.Move(webRootPath + fromPhoneClaimDir + fileName, webRootPath + fromPhoneClaimRejectedDir + fileName);
                            }

                            RV = 0;
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
    }
}
