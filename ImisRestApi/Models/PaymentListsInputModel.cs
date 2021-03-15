using ImisRestApi.ImisAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class PaymentListsInputModel
    {
        [Required]
        public string claim_administrator_code { get; set; }
        [ValidDate]
        public string last_update_date { get; set; }
    }
}