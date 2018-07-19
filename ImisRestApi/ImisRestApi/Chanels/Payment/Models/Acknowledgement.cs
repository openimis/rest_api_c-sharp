using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Chanels.Payment.Models
{
    public class Acknowledgement
    {
        [Required]
        public int PaymentId { get; set; }
        public string Description { get; set; }
    }
}