using OpenImis.DB.SqlServer;
using OpenImis.ModulesV2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenImis.ModulesV2.InsureeModule.Models
{
    public class FamilyModel
    {
        public int FamilyId { get; set; }
        public Guid FamilyUUID { get; set; }
        public IEnumerable<InsureeModel> Insurees { get; set; }
        public FamilySMS FamilySMS { get; set; }
        public int InsureeId { get; set; }
        public Guid InsureeUUID { get; set; }
        public int LocationId { get; set; }
        public bool Poverty { get; set; }
        public string FamilyType { get; set; }
        public string FamilyAddress { get; set; }
        public string Ethnicity { get; set; }
        public string ConfirmationNo { get; set; }
        public string ConfirmationType { get; set; }
        public bool IsOffline { get; set; }

        public static FamilyModel FromTblFamilies(TblFamilies tblFamilies)
        {
            FamilyModel familyModel = new FamilyModel()
            {
                FamilyId = tblFamilies.FamilyId,
                FamilyUUID = tblFamilies.FamilyUUID,
                InsureeId = tblFamilies.InsureeId,
                InsureeUUID = tblFamilies.Insuree.InsureeUUID,
                LocationId = TypeCast.GetValue<int>(tblFamilies.LocationId),
                Poverty = TypeCast.GetValue<bool>(tblFamilies.Poverty),
                FamilyType = tblFamilies.FamilyType,
                FamilyAddress = tblFamilies.FamilyAddress,
                Ethnicity = tblFamilies.Ethnicity,
                ConfirmationNo = tblFamilies.ConfirmationNo,
                ConfirmationType = tblFamilies.ConfirmationType,
                IsOffline = TypeCast.GetValue<bool>(tblFamilies.IsOffline),
                Insurees = tblFamilies.TblInsuree
                        .Where(i => i.ValidityTo == null)
                        .Select(i => InsureeModel.FromTblInsuree(i))
                        .ToList(),
                FamilySMS = tblFamilies.TblFamilySMS.Where(sms => sms.ValidityTo == null).Select(s => FamilySMS.FromTblFamilySMS(s)).LastOrDefault()
            };
            return familyModel;
        }
    }
}
