using ImisRestApi.ImisAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [Required(ErrorMessage = "3:Wrong or missing permanent village code")]
        public string VillageCode { get; set; }
        /// <summary>
        /// An insuree number belonging to the head of a family
        /// </summary>
        [Required(ErrorMessage = "1:Wrong format or missing insurance number of head")]
        [InsureeNumber(ErrorMessage = "1:Wrong format or missing insurance number of head")]
        public string HeadOfFamilyId { get; set; }
        [Required(ErrorMessage = "8:Missing other name",AllowEmptyStrings = false)]
        public string OtherName { get; set; }
        [Required(ErrorMessage = "7:Missing last name", AllowEmptyStrings = false)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "6:Wrong format or missing birth date")]
        [ValidDate]
        public string BirthDate { get; set; }
        [Required(ErrorMessage = "5:Wrong or missing gender"),StringLength(1,ErrorMessage = "5:Wrong or missing gender")]
        [BindRequired]
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