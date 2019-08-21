using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models.EnrollFamilyModels
{
    public class EnrollFamilyModel
    {
        public List<Family> Family { get; set; }
        public List<Insuree> Insuree { get; set; }
        public List<Policy> Policy { get; set; }
        public List<Premium> Premium { get; set; }
        public List<InsureePolicy> InsureePolicy { get; set; }
        public List<InsureeImage> Pictures { get; set; }


        public static explicit operator Enrolment(EnrollFamilyModel efm)
        {
            return new Enrolment()
            {
                FileInfo = new FileInfo(),
                Families = efm.Family,
                Insurees = efm.Insuree,
                Policies = efm.Policy,
                Premiums = efm.Premium,
                InsureePolicies = efm.InsureePolicy
            };
        }
    }
}
