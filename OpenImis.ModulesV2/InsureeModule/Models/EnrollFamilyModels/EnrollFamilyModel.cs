using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models.EnrollFamilyModels
{
    public class EnrollFamilyModel
    {
        public List<Family> Family { get; set; }

        public static explicit operator Enrolment(EnrollFamilyModel efm)
        {
            return new Enrolment()
            {
                FileInfo = new FileInfo(),
                Families = efm.Family.Select(x => new List<Family>()
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
                Insurees = efm.Family.Select(x => x.Insurees).FirstOrDefault().ToList(),
                Policies = efm.Family.Select(x => x.Policies).FirstOrDefault().ToList(),
                Premiums = efm.Family.Select(x => x.Policies.Select(s => s.Premium)).FirstOrDefault().ToList(),
                InsureePolicies = efm.Family.Select(x => x.InsureePolicy).FirstOrDefault().ToList(),
            };
        }
    }
}
