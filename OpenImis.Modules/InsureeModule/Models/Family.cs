using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Models
{
    public abstract class Family
    {
        public string InsuranceNumber { get; set; }
        public string OtherNames { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public bool? PoveryStatus { get; set; }
        public string ConfirmationType { get; set; }
        public string PermanentAddress { get; set; }
        public string MaritalStatus { get; set; }
        public bool BeneficiaryCard { get; set; }
        public string CurrentVillageCode { get; set; }
        public string CurrentAddress { get; set; }
        public string Profession { get; set; }
        public short? Education { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string FSPCode { get; set; }
    }
}
