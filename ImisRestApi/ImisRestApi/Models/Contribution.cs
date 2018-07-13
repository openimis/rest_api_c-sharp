using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImisRestApi.Models
{
    public class Contribution
    {
        [Required]
        public string InsuranceNumber { get; set; }
        [Required]
        public string Payer { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string ReceiptNumber { get; set; }
        [Required]
        public PaymentType PaymentType { get; set; }
        [Required]
        public ReactionType ReactionType { get; set; }
        public ContributionCategory ContributionCategory { get; set; }
    }
}