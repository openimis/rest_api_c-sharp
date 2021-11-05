using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models
{
    public class USSDGetCommissionInputs
    {
        [Required]
        public string enrolment_officer_code { get; set; }
        [Required]
        public string month { get; set; }
        [Required]
        public string year { get; set; }
        [Required]
        public CommissionMode mode { get; set; }
        public string insurance_product_code { get; set; }
        public string payer { get; set; }
    }
}
