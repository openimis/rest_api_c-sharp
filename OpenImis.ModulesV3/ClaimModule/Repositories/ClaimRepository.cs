using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.DB.SqlServer.DataHelper;
using OpenImis.ModulesV3.ClaimModule.Models;
using OpenImis.ModulesV3.ClaimModule.Models.RegisterClaim;
using OpenImis.ModulesV3.Helpers;

namespace OpenImis.ModulesV3.ClaimModule.Repositories
{
    public class ClaimRepository
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

                var fromPhoneClaimDir = _configuration["AppSettings:FromPhone_Claim"] + Path.DirectorySeparatorChar;
                var fromPhoneClaimRejectedDir = _configuration["AppSettings:FromPhone_Claim_Rejected"] + Path.DirectorySeparatorChar;

                var fileName = "Claim_" + claim.Details.HFCode + "_" + claim.Details.CHFID + "_" + claim.Details.ClaimCode + ".xml";

                var xmldoc = new XmlDocument();
                xmldoc.InnerXml = XML;

                try
                {

                    if (!Directory.Exists(fromPhoneClaimDir)) Directory.CreateDirectory(fromPhoneClaimDir);
                    if (!Directory.Exists(fromPhoneClaimRejectedDir)) Directory.CreateDirectory(fromPhoneClaimRejectedDir);

                    if (!File.Exists(fromPhoneClaimDir + fileName))
                    {
                        xmldoc.Save(fromPhoneClaimDir + fileName);
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
                        var claimRejectedParameter = new SqlParameter("@ClaimRejected", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                        var sql = "exec @RV = uspUpdateClaimFromPhone @XML, 0, @ClaimRejected OUTPUT";

                        DbConnection connection = imisContext.Database.GetDbConnection();

                        using (DbCommand cmd = connection.CreateCommand())
                        {
                            cmd.CommandText = sql;

                            cmd.Parameters.AddRange(new[] { xmlParameter, returnParameter, claimRejectedParameter });

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

                        int tempRV = (int)returnParameter.Value;
                        bool? isClaimRejected = claimRejectedParameter.Value as bool?;

                        if ((tempRV == 0) && (isClaimRejected == false))
                        {
                            RV = 1;
                        }
                        else if (tempRV == 0 && (isClaimRejected == true || isClaimRejected == null))
                        {
                            if (File.Exists(fromPhoneClaimDir + fileName) && !File.Exists(fromPhoneClaimRejectedDir + fileName))
                            {
                                File.Move(fromPhoneClaimDir + fileName, fromPhoneClaimRejectedDir + fileName);
                            }

                            RV = 0;
                        }
                        else
                        {
                            if (File.Exists(fromPhoneClaimDir + fileName))
                            {
                                File.Delete(fromPhoneClaimDir + fileName);
                            }

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

        public DiagnosisServiceItem GetDsi(DsiInputModel model)
        {
            DiagnosisServiceItem message = new DiagnosisServiceItem();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    List<CodeName> diagnoses;
                    List<CodeNamePrice> items;
                    List<CodeNamePrice> services;

                    diagnoses = imisContext.TblIcdcodes
                        .Where(i => (model.last_update_date == null || i.ValidityFrom >= model.last_update_date)
                            && i.ValidityTo == null)
                        .Select(x => new CodeName()
                        {
                            code = x.Icdcode,
                            name = x.Icdname
                        }).ToList();

                    items = imisContext.TblItems
                        .Where(i => (model.last_update_date == null || i.ValidityFrom >= model.last_update_date)
                            && i.ValidityTo == null)
                        .Select(x => new CodeNamePrice()
                        {
                            code = x.ItemCode,
                            name = x.ItemName,
                            price = x.ItemPrice.ToString()
                        }).ToList();

                    services = imisContext.TblServices
                        .Where(i => (model.last_update_date == null || i.ValidityFrom >= model.last_update_date)
                            && i.ValidityTo == null)
                        .Select(x => new CodeNamePrice()
                        {
                            code = x.ServCode,
                            name = x.ServName,
                            price = x.ServPrice.ToString()
                        }).ToList();

                    message.diagnoses = diagnoses;
                    message.items = items;
                    message.services = services;
                    message.update_since_last = DateTime.UtcNow;
                }

                return message;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ClaimAdminModel> GetClaimAdministrators()
        {
            List<ClaimAdminModel> response = new List<ClaimAdminModel>();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = imisContext.TblClaimAdmin
                        .Where(c => c.ValidityTo == null)
                        .Select(x => new ClaimAdminModel()
                        {
                            lastName = x.LastName,
                            otherNames = x.OtherNames,
                            claimAdminCode = x.ClaimAdminCode,
                            HFCode = x.Hf.Hfcode
                        }).ToList();
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TblControls> GetControls()
        {
            List<TblControls> response = new List<TblControls>();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    response = imisContext.TblControls
                        .Select(x => new TblControls()
                        {
                            FieldName = x.FieldName,
                            Adjustibility = x.Adjustibility,
                            Usage = x.Usage
                        }).ToList();
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PaymentLists GetPaymentLists(PaymentListsInputModel model)
        {
            PaymentLists message = new PaymentLists();

            try
            {
                using (var imisContext = new ImisDB())
                {
                    int? HFID;
                    int? PLServiceID;
                    int? PLItemID;

                    List<CodeName> hf = new List<CodeName>();
                    List<CodeNamePrice> plItemsDetail = new List<CodeNamePrice>();
                    List<CodeNamePrice> plServicesDetail = new List<CodeNamePrice>();

                    HFID = imisContext.TblClaimAdmin
                        .Where(c => c.ClaimAdminCode == model.claim_administrator_code
                            && c.ValidityTo == null)
                        .Select(x => x.Hfid).FirstOrDefault();

                    PLServiceID = imisContext.TblHf
                        .Where(h => h.HfId == HFID)
                        .Select(x => x.PlserviceId).FirstOrDefault();

                    PLItemID = imisContext.TblHf
                        .Where(h => h.HfId == HFID)
                        .Select(x => x.PlitemId).FirstOrDefault();

                    hf = imisContext.TblHf
                        .Where(h => h.HfId == HFID)
                        .Select(x => new CodeName()
                        {
                            code = x.Hfcode,
                            name = x.Hfname
                        }).ToList();

                    plItemsDetail = imisContext.TblPlitemsDetail
                            .Join(imisContext.TblItems,
                                p => p.ItemId,
                                i => i.ItemId,
                                (p, i) => new { TblPlitemsDetail = p, TblItems = i })
                            .Where(r => r.TblItems.ValidityTo == null)
                            .Where(r => r.TblPlitemsDetail.PlitemId == PLItemID
                                && r.TblPlitemsDetail.ValidityTo == null
                                && (model.last_update_date == null || r.TblPlitemsDetail.ValidityFrom >= model.last_update_date))
                            .Select(x => new CodeNamePrice()
                            {
                                code = x.TblItems.ItemCode,
                                name = x.TblItems.ItemName,
                                price = (x.TblPlitemsDetail.PriceOverule == null) ? x.TblItems.ItemPrice.ToString() : x.TblPlitemsDetail.PriceOverule.ToString()
                            }).ToList();

                    plServicesDetail = imisContext.TblPlservicesDetail
                            .Join(imisContext.TblServices,
                                p => p.ServiceId,
                                i => i.ServiceId,
                                (p, i) => new { TblPlservicesDetail = p, TblServices = i })
                            .Where(r => r.TblServices.ValidityTo == null)
                            .Where(r => r.TblPlservicesDetail.PlserviceId == PLServiceID
                                && r.TblPlservicesDetail.ValidityTo == null
                                && (model.last_update_date == null || r.TblPlservicesDetail.ValidityFrom >= model.last_update_date))
                            .Select(x => new CodeNamePrice()
                            {
                                code = x.TblServices.ServCode,
                                name = x.TblServices.ServName,
                                price = (x.TblPlservicesDetail.PriceOverule == null) ? x.TblServices.ServPrice.ToString() : x.TblPlservicesDetail.PriceOverule.ToString()
                            }).ToList();

                    message.health_facility_code = hf.Select(x => x.code).FirstOrDefault();
                    message.health_facility_name = hf.Select(x => x.name).FirstOrDefault();

                    message.pricelist_items = plItemsDetail;
                    message.pricelist_services = plServicesDetail;
                    message.update_since_last = DateTime.UtcNow;
                }

                return message;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ClaimOutput> GetClaims(ClaimsModel model)
        {
            string sSQL = @"exec uspAPIGetClaims @ClaimAdminCode,@StartDate,@EndDate,@DateProcessedFrom,@DateProcessedTo,@ClaimStatus";

            DataHelper helper = new DataHelper(_configuration);
            int? claimStatus = null;

            if (model.status_claim != 0)
                claimStatus = (int)model.status_claim;

            SqlParameter[] sqlParameters = {
                new SqlParameter("@ClaimAdminCode", model.claim_administrator_code),
                new SqlParameter("@StartDate",SqlDbType.DateTime){ Value = (model.visit_date_from != null)?model.visit_date_from:(object)DBNull.Value},
                new SqlParameter("@EndDate",SqlDbType.DateTime){ Value = (model.visit_date_to != null)?model.visit_date_to:(object)DBNull.Value},
                new SqlParameter("@DateProcessedFrom",SqlDbType.DateTime){ Value = (model.processed_date_from != null)?model.processed_date_from:(object)DBNull.Value},
                new SqlParameter("@DateProcessedTo", SqlDbType.DateTime){ Value = (model.processed_date_to != null)?model.processed_date_to:(object)DBNull.Value},
                new SqlParameter("@ClaimStatus", SqlDbType.Int){ Value = (claimStatus != null)?claimStatus:(object)DBNull.Value},
            };

            try
            {
                var response = helper.GetDataSet(sSQL, sqlParameters, CommandType.Text);

                DataTable responseItems = response.Tables[0];
                DataTable responseServices = response.Tables[1];
                DataTable responseClaims = response.Tables[2];

                Dictionary<string, ClaimOutput> admin_claims = (from DataRow dr in responseClaims.Rows
                                                                select new ClaimOutput()
                                                                {
                                                                    claim_uuid = dr["claim_uuid"].ToStringWithDBNull(),
                                                                    health_facility_code = dr["health_facility_code"].ToStringWithDBNull(),
                                                                    health_facility_name = dr["health_facility_name"].ToStringWithDBNull(),
                                                                    insurance_number = dr["insurance_number"].ToStringWithDBNull(),
                                                                    patient_name = dr["patient_name"].ToStringWithDBNull(),
                                                                    main_dg = dr["main_dg"].ToStringWithDBNull(),
                                                                    claim_number = dr["claim_number"].ToStringWithDBNull(),
                                                                    date_claimed = DateTime.Parse(dr["date_claimed"].ToStringWithDBNull()),
                                                                    visit_date_from = DateTime.Parse(dr["visit_date_from"].ToStringWithDBNull()),
                                                                    visit_type = dr["visit_type"].ToStringWithDBNull(),
                                                                    claim_status = dr["claim_status"].ToStringWithDBNull(),
                                                                    sec_dg_1 = dr["sec_dg_1"].ToStringWithDBNull(),
                                                                    sec_dg_2 = dr["sec_dg_2"].ToStringWithDBNull(),
                                                                    sec_dg_3 = dr["sec_dg_3"].ToStringWithDBNull(),
                                                                    sec_dg_4 = dr["sec_dg_4"].ToStringWithDBNull(),
                                                                    visit_date_to = DateTime.Parse(dr["visit_date_to"].ToStringWithDBNull()),
                                                                    claimed = dr["claimed"].ToString().ParseNullableDecimal(),
                                                                    approved = dr["approved"].ToString().ParseNullableDecimal(),
                                                                    adjusted = dr["adjusted"].ToString().ParseNullableDecimal(),
                                                                    explanation = dr["explanation"].ToStringWithDBNull(),
                                                                    adjustment = dr["adjustment"].ToStringWithDBNull(),
                                                                    guarantee_number = dr["guarantee_number"].ToStringWithDBNull(),
                                                                    services = new List<ClaimService>(),
                                                                    items = new List<ClaimItem>()
                                                                }).ToList().ToDictionary(x => x.claim_uuid, x => x, StringComparer.OrdinalIgnoreCase);

                (from DataRow dr in responseItems.Rows
                 select new ClaimItem()
                 {
                     claim_uuid = dr["claim_uuid"].ToStringWithDBNull(),
                     claim_number = dr["claim_number"].ToStringWithDBNull(),
                     item = dr["item"].ToStringWithDBNull(),
                     item_code = dr["item_code"].ToStringWithDBNull(),
                     item_qty = dr["item_qty"].ToString().ParseNullableDecimal(),
                     item_price = dr["item_price"].ToString().ParseNullableDecimal(),
                     item_adjusted_qty = dr["item_adjusted_qty"].ToString().ParseNullableDecimal(),
                     item_adjusted_price = dr["item_adjusted_price"].ToString().ParseNullableDecimal(),
                     item_explination = dr["item_explination"].ToStringWithDBNull(),
                     item_justificaion = dr["item_justificaion"].ToStringWithDBNull(),
                     item_valuated = dr["item_valuated"].ToString().ParseNullableDecimal(),
                     item_result = dr["item_result"].ToStringWithDBNull(),
                 }).ToList().ForEach((x) => { if (admin_claims.ContainsKey(x.claim_uuid)) admin_claims[x.claim_uuid].items.Add(x); });

                (from DataRow dr in responseServices.Rows
                 select new ClaimService()
                 {
                     claim_uuid = dr["claim_uuid"].ToStringWithDBNull(),
                     claim_number = dr["claim_number"].ToStringWithDBNull(),
                     service = dr["service"].ToStringWithDBNull(),
                     service_code = dr["service_code"].ToStringWithDBNull(),
                     service_qty = dr["service_qty"].ToString().ParseNullableDecimal(),
                     service_price = dr["service_price"].ToString().ParseNullableDecimal(),
                     service_adjusted_qty = dr["service_adjusted_qty"].ToString().ParseNullableDecimal(),
                     service_adjusted_price = dr["service_adjusted_price"].ToString().ParseNullableDecimal(),
                     service_explination = dr["service_explination"].ToStringWithDBNull(),
                     service_justificaion = dr["service_justificaion"].ToStringWithDBNull(),
                     service_valuated = dr["service_valuated"].ToString().ParseNullableDecimal(),
                     service_result = dr["service_result"].ToStringWithDBNull(),
                 }).ToList().ForEach((x) => { if (admin_claims.ContainsKey(x.claim_uuid)) admin_claims[x.claim_uuid].services.Add(x); });

                List<ClaimOutput> output = admin_claims.Values.ToList();

                return output;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
