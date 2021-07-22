using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels
{

    public class FamilyVM
    {
        public int FamilyId { get; set; }
        public Guid FamilyUUID { get; set; }
    }

    public class InsureeVM
    {
        public int InsureeId { get; set; }
        public Guid InsureeUUID { get; set; }
    }

    public class PolicyVM
    {
        public PolicyVM()
        {
            Premium = new List<PremiumVM>();
        }
        public int PolicyId { get; set; }
        public Guid PolicyUUID { get; set; }
        public List<PremiumVM> Premium { get; set; }
    }

    public class PremiumVM
    {
        public int PremiumId { get; set; }
        public Guid PremiumUUID { get; set; }
    }

    public class NewFamilyResponse
    {
        public NewFamilyResponse()
        {
            Family = new List<FamilyVM>();
            Insurees = new List<InsureeVM>();
            Policies = new List<PolicyVM>();
        }
        public int Response { get; set; }
        public List<FamilyVM> Family { get; set; }
        public List<InsureeVM> Insurees { get; set; }
        public List<PolicyVM> Policies { get; set; }
    }
}
