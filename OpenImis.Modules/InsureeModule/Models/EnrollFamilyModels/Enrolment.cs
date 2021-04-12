using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Models.EnrollFamilyModels
{
    public class Enrolment
    {
        public FileInfo FileInfo { get; set; }
        public List<FamilyMv> Families { get; set; }
        public List<Insuree> Insurees { get; set; }
        public List<PolicyMv> Policies { get; set; }
        public List<Premium> Premiums { get; set; }
        public List<InsureePolicy> InsureePolicies { get; set; }
    }
}
