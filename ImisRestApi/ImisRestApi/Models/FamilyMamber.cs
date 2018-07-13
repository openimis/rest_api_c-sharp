using System;
using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class FamilyMamber
    {
        [Required]
        [MaxLength(11)]
        public string InsureeNumber { get; set; }
        [Required]
        [MaxLength(11)]
        public string HeadInsureeNumber { get; set; }
        [Required]
        public string OtherName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }

        public string Relationship { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
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