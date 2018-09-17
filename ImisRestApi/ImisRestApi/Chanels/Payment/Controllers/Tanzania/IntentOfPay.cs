using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Chanels
{
    public class IntentOfPay
    {
        public string PhoneNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public string OfficerCode { get; set; }
        public string InsureeNumber { get; set; } 
        public string ProductCode { get; set; }   
        public EnrolmentType EnrolmentType { get; set; } 

    }
}
