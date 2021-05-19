using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    public class WebMatchModel
    {
        public string internal_identifier { get; set; }
        [Required]
        public int audit_user_id { get; set; }
        public string api_key { get; set; }
    }
}
