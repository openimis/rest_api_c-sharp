using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Models
{
    public class GetCommissionInputs
    {
        [Required]
        public string enrolment_officer_code { get; set; }
        [Required]
        public string month { get; set; }
        [Required]
        public string year { get; set; }
        [Required]
        public CommissionMode mode { get; set; }
        public string insrance_product_code { get; set; }
        public string payer { get; set; }
    }
}
