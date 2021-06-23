using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels
{
    public class EnrollFamilyModel
    {
        public List<Family> Family { get; set; }

        public Enrolment GetEnrolmentFromModel()
        {
            return new Enrolment()
            {
                FileInfo = new FileInfo(),
                Families = Family.Select(x => new List<Family>()
                {
                    new Family()
                    {
                        FamilyId = x.FamilyId,
                        InsureeId = x.InsureeId,
                        LocationId = x.LocationId,
                        HOFCHFID = x.HOFCHFID,
                        Poverty = x.Poverty,
                        FamilyType = x.FamilyType,
                        FamilyAddress = x.FamilyAddress,
                        Ethnicity = x.Ethnicity,
                        ConfirmationNo = x.ConfirmationNo,
                        ConfirmationType = x.ConfirmationType,
                        isOffline = x.isOffline
                    }
                }).FirstOrDefault(),
                Insurees = Family.Select(x => x.Insurees).FirstOrDefault().ToList(),
                Policies = Family.Select(x => x.Policies).FirstOrDefault().ToList(),
                Premiums = Family.Select(x => x.Policies.Select(s => s.Premium)).FirstOrDefault().ToList(),
                InsureePolicies = Family.Select(x => x.InsureePolicy).FirstOrDefault().ToList(),
            };
        }
    }
}
