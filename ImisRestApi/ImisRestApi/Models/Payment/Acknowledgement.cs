using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class Acknowledgement
    {
        [Required]
        public string InternalIdentifier { get; set; }
        public string Description { get; set; }
        public bool Success { get; set; }
    }
}