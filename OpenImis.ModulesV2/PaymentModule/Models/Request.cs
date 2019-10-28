using System.ComponentModel.DataAnnotations;

namespace OpenImis.ModulesV2.PaymentModule.Models
{
    public class Request
    {
        [Required(ErrorMessage = "1-  Wrong format of internal identifier")]
        public string internal_identifier { get; set; }
    }
}
