﻿using ImisRestApi.Chanels.Payment.Models;
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

        public string PhoneNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string OfficerCode { get; set; }
        public List<PaymentDetail> PaymentDetails { get; set; }
        
    }
}
