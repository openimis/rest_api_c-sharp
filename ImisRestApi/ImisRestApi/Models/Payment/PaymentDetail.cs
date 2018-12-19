using ImisRestApi.Chanels.Payment.Models;
using ImisRestApi.ImisAttributes;
using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class PaymentDetail
    {
        [InsureeNumber]
        public string InsureeNumber { get; set; }
        [Required(ErrorMessage = "2:Not valid insurance or missing product code")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "10:EnrolmentType was not provided")]
        [Range(0,2)]
        public EnrolmentType? Renewal { get; set; }
        public int IsRenewal()
        {
            switch (Renewal)
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