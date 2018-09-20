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
    }
}
