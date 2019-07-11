using OpenImis.Modules.Helpers.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Models
{
    public class EditFamilyMember
    {
        [Required]
        [InsureeNumber(ErrorMessage = "3:Wrong format or missing insurance number  of member")]
        public string InsureeNumber { get; set; }
        public string OtherName { get; set; }
        public string LastName { get; set; }
        [ValidDate(ErrorMessage = "5:Wrong format or missing birth date")]
        public string BirthDate { get; set; }
        [StringLength(1, ErrorMessage = "6:Wrong gender")]
        public string Gender { get; set; }
        public string Relationship { get; set; }
        [StringLength(1, ErrorMessage = "7:Wrong marital status")]
        public string MaritalStatus { get; set; }
        public bool Beneficiary_Card { get; set; }
        public string CurrentVillageCode { get; set; }
        public string CurrentAddressDetails { get; set; }
        public string Profession { get; set; }
        public string Education { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string FspCode { get; set; }
    }
}
