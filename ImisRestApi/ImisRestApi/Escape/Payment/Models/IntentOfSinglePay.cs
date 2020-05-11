using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ImisRestApi.ImisAttributes;
using ImisRestApi.Models;
using Newtonsoft.Json;

namespace ImisRestApi.Chanels.Payment.Models
{
    public class IntentOfSinglePay:IntentOfPay
    {
       
        [Required]
        public string Msisdn { get; set; }
        public override string phone_number { get; set; } = "+255";
        public string OfficerCode { get; set; }
        [Required]
        [InsureeNumber]
        public string InsureeNumber { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [ReqNoOfficer("EnrolmentType")]
        public EnrolmentType? EnrolmentType { get; set; }
        [JsonIgnore]
        public override List<PaymentDetail> policies { get => base.policies; set => base.policies = value; }

        public void SetDetails()
        {
            
            List<PaymentDetail> details = new List<PaymentDetail>();
            PaymentDetail detail = new PaymentDetail() { insurance_number = this.InsureeNumber, insurance_product_code = this.ProductCode, renewal = this.EnrolmentType - 1 };
            details.Add(detail);
            policies = details;
        }
    }
}
