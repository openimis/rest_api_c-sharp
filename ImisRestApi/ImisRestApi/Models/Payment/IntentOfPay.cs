using ImisRestApi.ImisAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public class IntentOfPay
    {
        [Required(ErrorMessage = "009: Phone number not provided")]
        public virtual string PhoneNumber { get; set; }
        public DateTime RequestDate { get; set; }
        [OfficerCode]
        public string OfficerCode { get; set; }
        public virtual List<PaymentDetail> PaymentDetails { get; set; }
        public decimal Amount { get; set; }
    }
}
