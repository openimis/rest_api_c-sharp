using ImisRestApi.ImisAttributes;
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
        [InsureeNumber(ErrorMessage ="1:Wrong format or missing insurance number ")]
        public string InsuranceNumber { get; set; }
        [Required(ErrorMessage = "7-Wrong or missing payer")]
        public string Payer { get; set; }
        [Required(ErrorMessage = "4:Wrong or missing payment date")]
        [ValidDate(ErrorMessage = "4:Wrong or missing payment date")]
        public string PaymentDate { get; set; }
        [Required(ErrorMessage = "3:Wrong or missing  product ")]
        public string ProductCode { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "8:Missing receipt no.")]
        public string ReceiptNumber { get; set; }
        [Required(ErrorMessage = "6:Wrong or missing payment type")]
        public PaymentType PaymentType { get; set; }
        [Required]
        public ReactionType ReactionType { get; set; }
        public ContributionCategory ContributionCategory { get; set; }
    }
}