using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.ModulesV3.PolicyModule.Models
{
    public class SelfRenewal
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string InsuranceNumber { get; set; }

        [Required]
        public string ProductCode { get; set; }
    }
}
