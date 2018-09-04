using ImisRestApi.Chanels.Payment.Models;
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
        public string InsureeNumber { get; set; } //CHF specific
        public string ProductCode { get; set; }   //CHF specific
        public EnrolmentType EnrolmentType { get; set; } //CHF specific

        public List<PaymentDetail> PaymentDetails { get; set; }
        public int IsRenewal()
        {
            switch (EnrolmentType)
            {
                case EnrolmentType.Renewal:
                    return 1;
                case EnrolmentType.New:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}
