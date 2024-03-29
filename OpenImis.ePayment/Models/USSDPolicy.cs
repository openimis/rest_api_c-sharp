﻿using OpenImis.ePayment.ImisAttributes;
using System.ComponentModel.DataAnnotations;

namespace OpenImis.ePayment.Models
{
    public class USSDPolicy
    {
        [Required]
        [InsureeNumber(ErrorMessage = "1:Wrong format or missing insurance number")]
        public string InsuranceNumber { get; set; }
        [Required(ErrorMessage = "4:Wrong or missing enrolment date")]
        [ValidDate(ErrorMessage = "4:Wrong or missing enrolment date")]
        public string Date { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string EnrollmentOfficerCode { get; set; }
    }
}