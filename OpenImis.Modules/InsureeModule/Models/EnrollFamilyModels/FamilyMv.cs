using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Models.EnrollFamilyModels
{
    public class FamilyMv
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
        public List<Insuree> Insurees { get; set; }
        public List<PolicyMv> Policies { get; set; }
        public List<InsureePolicy> InsureePolicy { get; set; }
    }
}
