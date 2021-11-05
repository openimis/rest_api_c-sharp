using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models
{
    public class MatchSms
    {
        public int PaymentId { get; set; }
        public DateTime? DateLastSms { get; set; }
        public DateTime? MatchedDate { get; set; }
    }
}
