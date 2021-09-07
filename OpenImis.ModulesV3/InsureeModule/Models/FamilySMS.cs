using OpenImis.DB.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models
{
    public class FamilySMS
    {
        public int FamilyId { get; set; }
        public bool ApprovalOfSMS { get; set; }
        public string LanguageOfSMS { get; set; }

        public static FamilySMS FromTblFamilySMS(TblFamilySMS tblFamilySMS)
        {
            if (tblFamilySMS == null)
            {
                return null;
            }

            FamilySMS familySMS = new FamilySMS()
            {
                FamilyId = tblFamilySMS.FamilyId,
                ApprovalOfSMS = tblFamilySMS.ApprovalOfSMS,
                LanguageOfSMS = tblFamilySMS.LanguageOfSMS
            };

            return familySMS;
        }
    }
}
