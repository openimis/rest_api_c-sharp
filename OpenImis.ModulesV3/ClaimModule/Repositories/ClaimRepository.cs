using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
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
                            if (File.Exists(fromPhoneClaimDir + fileName) && !File.Exists(fromPhoneClaimRejectedDir + fileName))
                            {
                                File.Move(fromPhoneClaimDir + fileName, fromPhoneClaimRejectedDir + fileName);
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
                        .Where(i => i.ValidityFrom >= Convert.ToDateTime(model.last_update_date)
                            && i.ValidityTo == null)
                        .Select(x => new CodeName()
                        {
                            code = x.Icdcode,
                            name = x.Icdname
                        }).ToList();

                    items = imisContext.TblItems
                        .Where(i => i.ValidityFrom >= Convert.ToDateTime(model.last_update_date)
                            && i.ValidityTo == null)
                        .Select(x => new CodeNamePrice()
                        {
                            code = x.ItemCode,
                            name = x.ItemName,
                            price = x.ItemPrice.ToString()
                        }).ToList();

                    services = imisContext.TblServices
                        .Where(i => i.ValidityFrom >= Convert.ToDateTime(model.last_update_date)
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
                            claimAdminCode = x.ClaimAdminCode
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
                                && (r.TblPlitemsDetail.ValidityFrom >= Convert.ToDateTime(model.last_update_date) || model.last_update_date == null))
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
                                && (r.TblPlservicesDetail.ValidityFrom >= Convert.ToDateTime(model.last_update_date) || model.last_update_date == null))
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
    }
}
