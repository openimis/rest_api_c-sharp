using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.ImisAttributes;
using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class PaymentDetail
    {
        public string InsureeNumber { get; set; }
        public string ProductCode { get; set; }
        public EnrolmentType PaymentType { get; set; }
        public int IsRenewal()
        {
            switch (PaymentType)
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