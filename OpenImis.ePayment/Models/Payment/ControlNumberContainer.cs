using System.ComponentModel.DataAnnotations;

namespace OpenImis.ePayment.Models
{
    public class ControlNumberContainer
    {
        [Required]
        public int InternalIdentifier { get; set; }
        public string ControlNumber { get; set; }
    }
}