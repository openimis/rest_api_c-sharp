using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.ModulesV3.ClaimModule.Models
{
    public class PaymentListsInputModel
    {
        [Required]
        public string claim_administrator_code { get; set; }
        //[ValidDate]
        public DateTime? last_update_date { get; set; }
    }
}
