using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class Acknowledgement
    {
        [Required]
        public string internal_identifier { get; set; }
        public string error_message { get; set; }
        public bool error_occured { get; set; }
    }
}