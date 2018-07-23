using ImisRestApi.ImisAttributes;
using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class PaymentDetail
    {
        [Required]
        [InsureeNumber]
        public string InsureeNumber { get; set; }
        public string ProductCode { get; set; }
        [Required]
        public bool Renewal { get; set; }
    }
}