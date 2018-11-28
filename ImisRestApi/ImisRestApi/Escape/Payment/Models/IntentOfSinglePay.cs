using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ImisRestApi.ImisAttributes;
using ImisRestApi.Models;
using Newtonsoft.Json;

namespace ImisRestApi.Chanels
{
    public class IntentOfSinglePay:IntentOfPay
    {
        [Required]
        public string Msisdn { get; set; }
        public override string PhoneNumber { get; set; } = "0000";
        [Required]
        [InsureeNumber]
        public string InsureeNumber { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public EnrolmentType? EnrolmentType { get; set; }
        [JsonIgnore]
        public override List<PaymentDetail> PaymentDetails { get => base.PaymentDetails; set => base.PaymentDetails = value; }

        public void SetDetails()
        {
            List<PaymentDetail> details = new List<PaymentDetail>();
            PaymentDetail detail = new PaymentDetail() { InsureeNumber = this.InsureeNumber, ProductCode = this.ProductCode, PaymentType = this.EnrolmentType };
            details.Add(detail);
            PaymentDetails = details;
        }
    }
}
