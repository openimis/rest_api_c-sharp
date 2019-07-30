using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.PaymentModule.Models.Response
{
    public class ErrorResponseV2
    {
        public bool error_occured { get; set; }
        public string error_message { get; set; }
    }
}
