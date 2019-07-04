using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Models
{
    public class MatchedPayment
    {
        public string FdMsg { get; set; }
        public string ProductCode { get; set; }
        public string PaymentId { get; set; }
        public string InsuranceNumber { get; set; }
        public string isActivated { get; set; }
        public int PaymentMatched { get; set; }
    }
}
