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
        [Required(ErrorMessage = "9: Phone number not provided")]
        public virtual string phone_number { get; set; }
        [ValidDate(ErrorMessage ="10: Request Date not valid")]
        public string request_date { get; set; }
        [OfficerCode]
        public string enrolment_officer_code { get; set; }
        public virtual List<PaymentDetail> policies { get; set; }
        [RequiredIfEo("amunt to be paid")]
        public decimal amount_to_be_paid { get; set; }
    }
}
