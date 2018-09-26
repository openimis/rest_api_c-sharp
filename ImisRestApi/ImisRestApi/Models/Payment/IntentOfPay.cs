using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public class IntentOfPay
    {
        [Required(ErrorMessage = "001: Phone number not provided")]
        public string PhoneNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string OfficerCode { get; set; }
        public virtual List<PaymentDetail> PaymentDetails { get; set; }

    }
}
