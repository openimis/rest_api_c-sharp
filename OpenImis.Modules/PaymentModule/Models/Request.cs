using System.ComponentModel.DataAnnotations;

namespace OpenImis.Modules.PaymentModule.Models
{
    public class Request
    {
        [Required(ErrorMessage = "1-Wrong format of internal identifier")]
        public string internal_identifier { get; set; }
    }
}
