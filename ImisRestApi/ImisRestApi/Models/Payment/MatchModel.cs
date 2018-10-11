using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Payment
{
    public class MatchModel
    {
        public int? PaymentId { get; set; }
        [Required]
        public int AuditUserId { get; set; }
    }
}
