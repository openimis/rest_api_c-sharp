using System;
using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class Policy
    {
        [Required]
        [MaxLength(11)]
        public string InsuranceNumber { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string EnrollmentOfficerCode { get; set; }
    }
}