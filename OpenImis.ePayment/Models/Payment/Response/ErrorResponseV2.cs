using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models.Payment.Response
{
    public class ErrorResponseV2
    {
        public bool error_occured { get; set; }
        public string error_message { get; set; }
    }
}
