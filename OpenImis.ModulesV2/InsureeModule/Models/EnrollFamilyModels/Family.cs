using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models.EnrollFamilyModels
{
    public class Family
    {
        public int FamilyId { get; set; }
        public int InsureeId { get; set; }
        public int LocationId { get; set; }
        public string HOFCHFID { get; set; }
        public bool Poverty { get; set; }
        public bool FamilyType { get; set; }
        public string FamilyAddress { get; set; }
        public string Ethnicity { get; set; }
        public string ConfirmationNo { get; set; }
        public string ConfirmationType { get; set; }
        public bool isOffline { get; set; }
    }
}
