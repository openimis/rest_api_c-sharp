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
    }
}