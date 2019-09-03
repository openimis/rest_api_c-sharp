using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.Helpers;
using OpenImis.ModulesV2.InsureeModule.Helpers;
using OpenImis.ModulesV2.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace OpenImis.ModulesV2.InsureeModule.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private IConfiguration _configuration;

        public PolicyRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<GetPolicyModel> Get(string officerCode)
        {
            List<GetPolicyModel> response = new List<GetPolicyModel>();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var officerCodeParameter = new SqlParameter("@OfficerCode", SqlDbType.NVarChar, 8);
                    officerCodeParameter.Value = officerCode;

                    var sql = "exec uspGetPolicyRenewals @OfficerCode";

                    DbConnection connection = imisContext.Database.GetDbConnection();

                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;

                        cmd.Parameters.AddRange(new[] { officerCodeParameter });

                        if (connection.State.Equals(ConnectionState.Closed)) connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                    response.Add(new GetPolicyModel()
                                    {
                                        RenewalId = int.Parse(reader["RenewalId"].ToString()),
                                        PolicyId = int.Parse(reader["PolicyId"].ToString()),
                                        OfficerId = int.Parse(reader["OfficerId"].ToString()),
                                        OfficerCode = reader["OfficerCode"].ToString(),
                                        CHFID = reader["CHFID"].ToString(),
                                        LastName = reader["LastName"].ToString(),
                                        OtherNames = reader["OtherNames"].ToString(),
                                        ProductCode = reader["ProductCode"].ToString(),
                                        ProductName = reader["ProductName"].ToString(),
                                        VillageName = reader["VillageName"].ToString(),
                                        RenewalPromptDate = reader["RenewalPromptDate"].ToString(),
                                        Phone = reader["Phone"].ToString()
                                    });
                                }
                            } while (reader.NextResult());
                        }
                    }
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

        public int Post(PolicyRenewalModel policy)
        {
            int RV = -99;

            try
            {
                var policyRenew = policy.GetPolicy();
                var XML = policyRenew.XMLSerialize();

                Debug.WriteLine(XML);

                string fileName = "test";

                using (var imisContext = new ImisDB())
                {
                    var xmlParameter = new SqlParameter("@XML", XML) { DbType = DbType.Xml };
                    var returnParameter = OutputParameter.CreateOutputParameter("@RV", SqlDbType.Int);
                    var fileNameParameter = new SqlParameter("@FileName", SqlDbType.NVarChar, 200);
                    fileNameParameter.Value = fileName;

                    var sql = "exec @RV = uspIsValidRenewal @FileName, @XML";

                    DbConnection connection = imisContext.Database.GetDbConnection();

                    using (DbCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;

                        cmd.Parameters.AddRange(new[] { fileNameParameter, xmlParameter, returnParameter });

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

        public int Delete(Guid uuid)
        {
            int response = 0;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var renewal = imisContext.TblPolicyRenewals.SingleOrDefault(pr => pr.RenewalUUID == uuid);

                    if (renewal == null) return -1;

                    renewal.ResponseStatus = 2;
                    renewal.ResponseDate = DateTime.Now;
                    imisContext.SaveChanges();
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
