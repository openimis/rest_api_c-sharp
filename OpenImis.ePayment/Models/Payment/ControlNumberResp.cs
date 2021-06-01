using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models.Payment
{
    public class ControlNumberResp
    {
        [Required]
        public int internal_identifier { get; set; }
        public string control_number { get; set; }
        [Required]
        public bool error_occured { get; set; }
        public string error_message { get; set; }
    }
}
