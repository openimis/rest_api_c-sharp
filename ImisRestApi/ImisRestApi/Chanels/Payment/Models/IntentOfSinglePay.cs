using System.Collections.Generic;
using ImisRestApi.Models;
using Newtonsoft.Json;

namespace ImisRestApi.Chanels
{
    public class IntentOfSinglePay:IntentOfPay
    {
        public string InsureeNumber { get; set; }
        public string ProductCode { get; set; }
        public EnrolmentType EnrolmentType { get; set; }
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
