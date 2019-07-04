using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Models
{
    public class ControlNumberResp
    {
        [Required]
        public string internal_identifier { get; set; }
        public string control_number { get; set; }
        [Required]
        public bool error_occured { get; set; }
        public string error_message { get; set; }
    }
}
