using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ImisRestApi.Models
{
    /// <summary>
    ///  IMIS Family Group
    /// </summary>
    public class Family
    {
        /// <summary>
        /// 4 digit string that represents a village
        /// </summary>
        [Required]
        [MaxLength(11, ErrorMessage = "The village Code must be 11 characters long.")]
        public string VillageCode { get; set; }
        /// <summary>
        /// An insuree number belonging to the head of a family
        /// </summary>
        [Required]
        [MaxLength(11, ErrorMessage = "The HeadOf Family Id must be 11 characters long.")]
        public string HeadOfFamilyId { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "OtherName must be 20 characters long.")]
        public string OtherName { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "LastName must be 20 characters long.")]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public Gender Gender { get; set; }
        
        public bool PovertyStatus { get; set; }
        public string ConfirmationType { get; set; }
        public string GroupType { get; set; }
        public string ConfrimationNo { get; set; }
        public string PermanentAddressDetails { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
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