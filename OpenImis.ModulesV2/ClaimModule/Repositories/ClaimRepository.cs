using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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

        public ClaimRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int Create(Claim claim)
        {
            try
            {
                var XML = claim.XMLSerialize();
                var RV = -99;

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

                    RV = (int)returnParameter.Value;
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
