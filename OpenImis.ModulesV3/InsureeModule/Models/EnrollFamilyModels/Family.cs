using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels
{
    public class Family
    {
        public int FamilyId { get; set; }
        public int InsureeId { get; set; }
        public int LocationId { get; set; }
        public string HOFCHFID { get; set; }
        public string Poverty { get; set; }
        public string FamilyType { get; set; }
        public string FamilyAddress { get; set; }
        public string Ethnicity { get; set; }
        public string ConfirmationNo { get; set; }
        public string ConfirmationType { get; set; }
        public string isOffline { get; set; }
        public string ApprovalOfSMS { get; set; }
        public string LanguageOfSMS { get; set; }
        public List<Insuree> Insurees { get; set; }
        public List<Policy> Policies { get; set; }
        public List<InsureePolicy> InsureePolicy { get; set; }
        public FamilySMS FamilySMS { get; set; }
    }
}
