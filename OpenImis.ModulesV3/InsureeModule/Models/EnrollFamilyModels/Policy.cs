using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels
{
    public class Policy
    {
        public int PolicyId { get; set; }
        public int FamilyId { get; set; }
        public DateTime? EnrollDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string PolicyStatus { get; set; }
        public string PolicyValue { get; set; }
        public int ProdId { get; set; }
        public int OfficerId { get; set; }
        public string PolicyStage { get; set; }
        public string isOffline { get; set; }
        public int ControlNumberId { get; set; }
        public List<Premium> Premium { get; set; }
    }
}
