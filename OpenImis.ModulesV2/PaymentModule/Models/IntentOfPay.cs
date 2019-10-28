using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenImis.ModulesV2.PaymentModule.Models
{
    public class IntentOfPay
    {
        [Required(ErrorMessage = "9: Phone number not provided")]
        public virtual string phone_number { get; set; }
        [DataType(DataType.DateTime)]
        public string request_date { get; set; }
        public string enrolment_officer_code { get; set; }
        public virtual List<PaymentDetail> policies { get; set; }
        public decimal amount_to_be_paid { get; set; }
        public string language { get; set; }
        [Range(0, 2, ErrorMessage = "10-Uknown type of payment")]
        public TypeOfPayment? type_of_payment { get; set; }
    }
}
