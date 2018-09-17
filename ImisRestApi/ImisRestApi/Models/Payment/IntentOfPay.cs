using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public class IntentOfPay
    {
        public string PhoneNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string OfficerCode { get; set; }
        public virtual List<PaymentDetail> PaymentDetails { get; set; }

    }
}
