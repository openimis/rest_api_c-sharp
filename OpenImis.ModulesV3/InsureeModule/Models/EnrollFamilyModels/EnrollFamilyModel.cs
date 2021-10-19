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

            foreach (Family f in Family) {
                // add the Family
                Family family = new Family()
                {
                    FamilyId = f.FamilyId,
                    InsureeId = f.InsureeId,
                    LocationId = f.LocationId,
                    HOFCHFID = f.HOFCHFID,
                    Poverty = f.Poverty,
                    FamilyType = f.FamilyType,
                    FamilyAddress = f.FamilyAddress,
                    Ethnicity = f.Ethnicity,
                    ConfirmationNo = f.ConfirmationNo,
                    ConfirmationType = f.ConfirmationType,
                    isOffline = f.isOffline,
                    FamilySMS = f.FamilySMS != null ? new FamilySMS()
                    {
                        FamilyId = f.FamilySMS.FamilyId,
                        ApprovalOfSMS = f.FamilySMS.ApprovalOfSMS,
                        LanguageOfSMS = f.FamilySMS.LanguageOfSMS
                    } : null
                };
                enrollment.Families.Add(family);
                // add the Insurees
                foreach (var i in f.Insurees)
                {
                    enrollment.Insurees.Add(i);
                }
                // add the Policies
                foreach (var p in f.Policies)
                {
                    enrollment.Policies.Add(p);
                    if (p.Premium != null)
                    {
                        foreach (var pr in p.Premium)
                        {
                            enrollment.Premiums.Add(pr);
                        }
                    }
                }
                // add PolicyInsuree
                foreach (var ip in f.InsureePolicy)
                {
                    enrollment.InsureePolicies.Add(ip);
                }

            }

            return enrollment;
        }
    }
}

