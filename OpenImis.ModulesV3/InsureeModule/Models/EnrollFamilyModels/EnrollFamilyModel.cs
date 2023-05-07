using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels
{
    public class EnrolFamilyModel
    {
        public List<Family> Family { get; set; }
        public string Source { get; set; } = "unknown";
        public string SourceVersion { get; set; } = "";

        public Enrolment GetEnrolmentFromModel()
        {

            var enrolment = new Enrolment();
            enrolment.FileInfo = new FileInfo();
            
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
                enrolment.Families.Add(family);
                // add the Insurees
                foreach (var i in f.Insurees)
                {
                    enrolment.Insurees.Add(i);
                }

                // add the Attachments
                /*foreach (var i in f.Attachments)
                {
                    enrolment.Attachments.Add(i);
                }*/

                // add the Policies
                foreach (var p in f.Policies)
                {
                    var pol = p;
                    enrolment.Policies.Add(p);
                    if (p.Premium != null)
                    {
                        foreach (var pr in p.Premium)
                        {
                            enrolment.Premiums.Add(pr);
                        }
                    }
                }
                // add PolicyInsuree
                foreach (var ip in f.InsureePolicy)
                {
                    enrolment.InsureePolicies.Add(ip);
                }

            }

            return enrolment;
        }
    }
}

