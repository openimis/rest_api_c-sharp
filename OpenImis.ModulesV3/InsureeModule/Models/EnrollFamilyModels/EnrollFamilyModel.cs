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

            var enrollment = new Enrolment();
            enrollment.FileInfo = new FileInfo();


            var families = Family.Select(x => new List<Family>()
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
                        isOffline = x.isOffline,
                        FamilySMS =  x.FamilySMS != null ? new FamilySMS() {
                            FamilyId = x.FamilySMS.FamilyId,
                            ApprovalOfSMS = x.FamilySMS.ApprovalOfSMS,
                            LanguageOfSMS = x.FamilySMS.LanguageOfSMS
                        } : null
                    }
                });

            foreach (var f in families)
            {
                enrollment.Families.Add(f.First());
            }

            var insurees = Family.Select(x => x.Insurees);
            foreach (var i in insurees)
            {
                enrollment.Insurees.Add(i.First());
            }

            var policies = Family.Select(x => x.Policies);
            foreach (var p in policies)
            {
                enrollment.Policies.Add(p.First());
                if (p.First().Premium != null)
                {
                    foreach (var pr in p.First().Premium)
                    {
                        enrollment.Premiums.Add(pr);
                    }
                }
            }

            var iPolicy = Family.Select(x => x.InsureePolicy);
            foreach (var ip in iPolicy)
            {
                enrollment.InsureePolicies.Add(ip.First());
            }

            return enrollment;
        }
    }
}

