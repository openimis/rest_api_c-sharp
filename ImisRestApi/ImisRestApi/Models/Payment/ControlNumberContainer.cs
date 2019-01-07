using System.ComponentModel.DataAnnotations;

namespace ImisRestApi.Models
{
    public class ControlNumberContainer
    {
        [Required]
        public int InternalIdentifier { get; set; }
        public string ControlNumber { get; set; }
    }
}