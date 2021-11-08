using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models.Payment
{
    public class PaymentRequest
    {
        public List<Request> requests { get; set; }
    }

    public class Request
    {
        [Required(ErrorMessage = "1-  Wrong format of internal identifier")]
        public string internal_identifier { get; set; }
    }
}
