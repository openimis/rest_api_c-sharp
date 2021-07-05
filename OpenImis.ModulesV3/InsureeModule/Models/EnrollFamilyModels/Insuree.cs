using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.InsureeModule.Models.EnrollFamilyModels
{
    public class Insuree
    {
        public int InsureeId { get; set; }
        public int FamilyId { get; set; }
        public string CHFID { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string Marital { get; set; }
        public string isHead { get; set; }
        public string IdentificationNumber { get; set; }
        public string Phone { get; set; }
        public string PhotoPath { get; set; }
        public string CardIssued { get; set; }
        public string Relationship { get; set; }
        public string Profession { get; set; }
        public string Education { get; set; }
        public string Email { get; set; }
        public string TypeOfId { get; set; }
        public string HFID { get; set; }
        public string CurrentAddress { get; set; }
        public string GeoLocation { get; set; }
        public string CurVillage { get; set; }
        public string isOffline { get; set; }
        public InsureeImage Picture { get; set; }
        public bool Vulnerability { get; set; }
    }
}
