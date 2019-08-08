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

        public Enrolment GetEnrolmentFromModel()
        {
            return new Enrolment()
            {
                FileInfo = new FileInfo(),
                Families = Family,
                Insurees = Insuree,
                Policies = Policy,
                Premiums = Premium,
                InsureePolicies = InsureePolicy
            };
        }
    }
}
