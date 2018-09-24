using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Payment
{
    public class MatchModel
    {
        public int? PaymentId { get; set; }
        public int AuditUserId { get; set; }
    }
}
