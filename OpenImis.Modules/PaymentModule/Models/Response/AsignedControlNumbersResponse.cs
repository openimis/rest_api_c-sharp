using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PaymentModule.Models.Response
{
    public class AsignedControlNumbersResponse
    {
        public bool error_occured { get; set; }
        public List<AssignedControlNumber> assigned_control_numbers { get; set; }
    }
}
