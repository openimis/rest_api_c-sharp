using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Models
{
    public class MatchModel
    {
        public string internal_identifier { get; set; }
        [Required]
        public int audit_user_id { get; set; }
    }
}
