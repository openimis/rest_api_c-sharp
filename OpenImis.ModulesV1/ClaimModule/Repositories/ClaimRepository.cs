using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV1.ClaimModule.Models;

namespace OpenImis.ModulesV1.ClaimModule.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        public ClaimRepository()
        {
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