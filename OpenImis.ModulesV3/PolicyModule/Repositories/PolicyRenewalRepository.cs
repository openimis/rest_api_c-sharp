using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV3.Helpers;
using OpenImis.ModulesV3.PolicyModule.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using System;
using System.Diagnostics;

namespace OpenImis.ModulesV3.PolicyModule.Repositories
{
    public class PolicyRenewalRepository : IPolicyRenewalRepository
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PolicyRenewalRepository(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // TODO: Receiving RenewalUUID directly from SP
        public List<GetPolicyRenewalModel> Get(string officerCode)
        {
            List<GetPolicyRenewalModel> response = new List<GetPolicyRenewalModel>();

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
                                    response.Add(new GetPolicyRenewalModel()
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
                                        Phone = reader["Phone"].ToString(),
                                        RenewalUUID = GetRenewalUUIDById(int.Parse(reader["RenewalId"].ToString()))
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

        // TODO Change the RV assignment codes. It should be on the list for better understanding
        public int Post(PolicyRenewalModel policy)
        {
            int RV = 2;

            try
            {
                var policyRenew = policy.GetPolicy();
                var XML = policyRenew.XMLSerialize();

                var fromPhoneRenewalDir = _configuration["AppSettings:FromPhone_Renewal"] + Path.DirectorySeparatorChar;
                var fromPhoneRenewalRejectedDir = _configuration["AppSettings:FromPhone_Renewal_Rejected"] + Path.DirectorySeparatorChar;

                var fileName = "RenPol_" + policy.Date + "_" + policy.CHFID + "_" + policy.ReceiptNo + ".xml";

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = XML;

                bool ifSaved = false;

                try
                {
                    if (!Directory.Exists(fromPhoneRenewalDir)) Directory.CreateDirectory(fromPhoneRenewalDir);
                    if (!Directory.Exists(fromPhoneRenewalRejectedDir)) Directory.CreateDirectory(fromPhoneRenewalRejectedDir);

                    xmldoc.Save(fromPhoneRenewalDir + fileName);
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

                        int tempRV = (int)returnParameter.Value;

                        if (tempRV == 0 || tempRV == -4)
                        {
                            RV = 1;
                        }
                        else if (tempRV == -1 || tempRV == -2 || tempRV == -3)
                        {
                            if (File.Exists(fromPhoneRenewalDir + fileName))
                            {
                                File.Move(fromPhoneRenewalDir + fileName, fromPhoneRenewalRejectedDir + fileName);
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
                    response = 1;
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

        private Guid GetRenewalUUIDById(int id)
        {
            Guid response;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = imisContext.TblPolicyRenewals
                        .Where(o => o.RenewalId == id)
                        .Select(x => x.RenewalUUID)
                        .FirstOrDefault();
                }

                return response;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public DataMessage GetCommissions(GetCommissionInputs model)
        {
            dynamic response;

            int year = DateTime.UtcNow.Year;
            int month = DateTime.UtcNow.Month;

            try
            {
                year = Convert.ToInt32(model.year);
                month = Convert.ToInt32(model.month);
            }
            catch (Exception)
            {
                throw;
            }

            DateTime minDate = new DateTime(year, month, 1);
            DateTime maxDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            DataMessage message;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var res = (from PR in imisContext.TblPremium
                               join P in imisContext.TblPolicy.Where(p => p.ValidityTo == null) on PR.PolicyId equals P.PolicyId
                               join R in imisContext.TblReporting on PR.ReportingCommissionID equals R.ReportingId
                               join O in imisContext.TblOfficer on P.OfficerId equals O.OfficerId
                               where (PR.ReportingCommissionID == null
                                    && (PR.PayDate >= minDate && PR.PayDate <= maxDate)
                                    && PR.ValidityTo == null
                                    && O.Code == model.enrolment_officer_code)
                               select new
                               {
                                   Commission = (R.CammissionRate == null ? 0.00M : R.CammissionRate) * PR.Amount,
                                   PR.Amount
                               })
                               .ToList();

                    var c = res.Count > 0 ? res.Sum(x => x.Commission) : null;
                    var a = res.Count > 0 ? res.Sum(x => (decimal?)x.Amount) : null;

                    response = new List<dynamic>() { new { Commission = c, Amount = a } };

                    message = new GetCommissionResponse(0, false, response, 0).Message;
                }
            }
            catch (Exception e)
            {
                message = new GetCommissionResponse(e).Message;
            }

            return message;
        }
    }
}
