using ImisRestApi.ImisAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class FamilyMamber
    {
        [Required]
        [InsureeNumber(ErrorMessage = "3:Wrong format or missing insurance number  of member")]
        public string InsureeNumber { get; set; }
        [Required]
        [InsureeNumber(ErrorMessage = "1:Wrong format or missing insurance number of head")]
        public string HeadInsureeNumber { get; set; }
        [Required(ErrorMessage = "7:Missing other name")]
        public string OtherName { get; set; }
        [Required(ErrorMessage = "6:Missing last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "5:Wrong format or missing birth date")]
        [ValidDate]
        public string BirthDate { get; set; }
        [StringLength(1, ErrorMessage = "11:Wrong marital status")]
        public string Gender { get; set; }

        public string Relationship { get; set; }
        [StringLength(1, ErrorMessage = "11:Wrong marital status")]
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