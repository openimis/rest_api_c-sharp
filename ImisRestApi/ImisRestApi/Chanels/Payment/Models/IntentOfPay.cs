using ImisRestApi.ImisAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public class IntentOfPay
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime Request_Date { get; set; }
        public string EnrolmentOfficerCode { get; set; }
        [Required]
        [IsPayment]
        public List<PaymentDetail> PaymentDetails { get; set; }
    }
}
