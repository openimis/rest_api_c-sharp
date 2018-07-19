using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Chanels.Payment.Models
{
    public class ControlNumberContainer
    {
        [Required]
        public int PaymentId { get; set; }
        public string ControlNumber { get; set; }
    }
}