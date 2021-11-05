using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels
{
    public class Enrolment
    {

        public Enrolment()
        {
            Families = new List<Family>();
            Insurees = new List<Insuree>();
            Policies = new List<Policy>();
            Premiums = new List<Premium>();
            InsureePolicies = new List<InsureePolicy>();
        }

        public FileInfo FileInfo { get; set; }
        public List<Family> Families { get; set; }
        public List<Insuree> Insurees { get; set; }
        public List<Policy> Policies { get; set; }
        public List<Premium> Premiums { get; set; }
        public List<InsureePolicy> InsureePolicies { get; set; }
    }
}
