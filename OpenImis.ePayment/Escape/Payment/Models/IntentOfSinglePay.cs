﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenImis.ePayment.ImisAttributes;
using OpenImis.ePayment.Models;
using Newtonsoft.Json;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    public class IntentOfSinglePay:IntentOfPay
    {
       
        [Required]
        public string Msisdn { get; set; } // Mobile Station International Subscriber Directory Number
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
