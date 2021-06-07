using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models.EnrollFamilyModels
{
    public class FamilySMS
    {
        public int FamilyId { get; set; }
        public string ApprovalOfSMS { get; set; }
        public string LanguageOfSMS { get; set; }
    }
}
