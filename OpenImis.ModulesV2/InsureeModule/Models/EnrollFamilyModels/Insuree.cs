using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models.EnrollFamilyModels
{
    public class Insuree
    {
        public int InsureeId { get; set; }
        public int FamilyId { get; set; }
        public string CHFID { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public DateTime DOB { get; set; }
        public int Gender { get; set; }
        public int Marital { get; set; }
        public string isHead { get; set; }
        public string IdentificationNumber { get; set; }
        public string Phone { get; set; }
        public string PhotoPath { get; set; }
        public bool CardIssued { get; set; }
        public int Relationship { get; set; }
        public int Profession { get; set; }
        public int Education { get; set; }
        public string Email { get; set; }
        public int TypeOfId { get; set; }
        public int HFID { get; set; }
        public string CurrentAddress { get; set; }
        public string GeoLocation { get; set; }
        public int CurVillage { get; set; }
        public bool isOffline { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
