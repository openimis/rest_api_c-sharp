using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Models
{
    public class MatchSms
    {
        public int PaymentId { get; set; }
        public DateTime? DateLastSms { get; set; }
        public DateTime? MatchedDate { get; set; }
    }
}
