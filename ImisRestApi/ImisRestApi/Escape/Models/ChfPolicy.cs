using ImisRestApi.ImisAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Escape.Models
{
    public class ChfPolicy
    {
        [Required]
        [InsureeNumber(ErrorMessage = "1:Wrong format or missing insurance number")]
        public string InsuranceNumber { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string OfficerCode { get; set; }
    }
}
