using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models.Payment
{
    public class MatchModel
    {
        public int internal_identifier { get; set; }
        [Required]
        public int audit_user_id { get; set; }
    }
}
