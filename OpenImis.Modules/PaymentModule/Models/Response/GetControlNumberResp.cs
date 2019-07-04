using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Models.Response
{
    public class GetControlNumberResp
    {
        public bool error_occured { get; set; }
        public string error_message { get; set; }
        public string internal_identifier { get; set; }
        public string control_number { get; set; }
    }
}
