using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.PaymentModule.Models.Response
{
    public class AssignedControlNumber
    {
        public string internal_identifier { get; set; }
        public string control_number { get; set; }
    }
}
