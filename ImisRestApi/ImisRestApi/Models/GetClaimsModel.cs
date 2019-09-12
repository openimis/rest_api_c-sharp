using ImisRestApi.ImisAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public class GetClaimsModel
    {
        [Required]
        public string claim_administrator_code { get; set; }
        public string status_claim { get; set; }
        [ValidDate(ErrorMessage = "4:Wrong or missing enrolment date")]
        public string visit_date_from { get; set; }
        [ValidDate(ErrorMessage = "4:Wrong or missing enrolment date")]
        public string visit_date_to { get; set; }
        [ValidDate(ErrorMessage = "4:Wrong or missing enrolment date")]
        public string processed_date_from { get; set; }
        [ValidDate(ErrorMessage = "4:Wrong or missing enrolment date")]
        public string processed_date_to { get; set; }
    }
}
