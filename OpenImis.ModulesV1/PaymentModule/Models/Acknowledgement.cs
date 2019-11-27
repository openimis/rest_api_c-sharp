using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.ModulesV1.PaymentModule.Models
{
    public class Acknowledgement
    {
        [Required]
        public string internal_identifier { get; set; }
        public string error_message { get; set; }
        public bool error_occured { get; set; }
    }
}
