using ImisRestApi.ImisAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public class EditFamily
    {
        public string VillageCode { get; set; }
        [Required(ErrorMessage = "1:Wrong format or missing insurance number of head")]
        [InsureeNumber(ErrorMessage = "1:Wrong format or missing insurance number of head")]
        public string HeadOfFamilyId { get; set; }
        public string OtherName { get; set; }
        public string LastName { get; set; }
        [ValidDate(ErrorMessage = "6:Wrong format or missing birth date")]
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public bool PovertyStatus { get; set; }
        public string ConfirmationType { get; set; }
        public string GroupType { get; set; }
        public string ConfrimationNo { get; set; }
        public string PermanentAddressDetails { get; set; }
        [StringLength(1, ErrorMessage = "11:Wrong marital status")]
        public string MaritalStatus { get; set; }
        public bool BeneficiaryCard { get; set; }
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
