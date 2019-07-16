using OpenImis.Modules.Helpers.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Models
{
    public abstract class Family
    {
        public string InsuranceNumber { get; set; }
        public string OtherNames { get; set; }

        [Required(ErrorMessage = "7:Missing last name", AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "6:Wrong format or missing birth date")]
        [ValidDate]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "5:Wrong or missing gender"), StringLength(1, ErrorMessage = "5:Wrong or missing gender")]
        public string Gender { get; set; }

        public bool? PoveryStatus { get; set; }
        public string ConfirmationType { get; set; }
        public string PermanentAddress { get; set; }

        [StringLength(1, ErrorMessage = "11:Wrong marital status")]
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
