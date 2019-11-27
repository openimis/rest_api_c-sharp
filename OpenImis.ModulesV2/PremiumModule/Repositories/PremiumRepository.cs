using Microsoft.Extensions.Configuration;
using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.PremiumModule.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OpenImis.ModulesV2.PremiumModule.Repositories
{
    public class PremiumRepository : IPremiumRepository
    {
        private IConfiguration _configuration;

        public PremiumRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Get(ReceiptRequestModel receipt)
        {
            bool response = false;

            try
            {
                using (var imisContext = new ImisDB())
                {
                    var districtId = (from F in imisContext.TblFamilies
                                      join I in imisContext.TblInsuree on F.InsureeId equals I.InsureeId
                                      join V in imisContext.TblVillages on F.LocationId equals V.VillageId
                                      join W in imisContext.TblWards on V.WardId equals W.WardId
                                      join D in imisContext.TblDistricts on W.DistrictId equals D.DistrictId
                                      where (F.ValidityTo == null
                                      && I.ValidityTo == null
                                      && I.Chfid == receipt.CHFID)
                                      select D.DistrictId)
                                   .FirstOrDefault();

                    int? premiumId = (from PR in imisContext.TblPremium
                                      join PL in imisContext.TblPolicy on PR.PolicyId equals PL.PolicyId
                                      join F in imisContext.TblFamilies on PL.FamilyId equals F.FamilyId
                                      join I in imisContext.TblInsuree on F.InsureeId equals I.InsureeId
                                      join V in imisContext.TblVillages on F.LocationId equals V.VillageId
                                      join W in imisContext.TblWards on V.WardId equals W.WardId
                                      join D in imisContext.TblDistricts on W.DistrictId equals D.DistrictId
                                      where (PR.ValidityTo == null
                                      && PL.ValidityTo == null
                                      && F.ValidityTo == null
                                      && I.ValidityTo == null
                                      && D.ValidityTo == null
                                      && PR.Receipt == receipt.ReceiptNo
                                      && D.DistrictId == districtId)
                                      select PR.PremiumId)
                                   .FirstOrDefault();

                    if (premiumId == 0)
                    {
                        response = true;
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
    }
}
