using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.ModulesV3.PolicyModule.Models
{
    public class GetCommissionInputs
    {
        public string enrolment_officer_code { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public CommissionMode mode { get; set; }
        public string insrance_product_code { get; set; }
        public string payer { get; set; }
    }
}
