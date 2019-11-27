using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Models
{
    public class Policy
    {
        [Required]
        //[InsureeNumber(ErrorMessage = "1:Wrong format or missing insurance number")]
        public string InsuranceNumber { get; set; }
        [Required(ErrorMessage = "4:Wrong or missing enrolment date")]
        //[ValidDate(ErrorMessage = "4:Wrong or missing enrolment date")]
        public string Date { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string EnrollmentOfficerCode { get; set; }
    }
}
