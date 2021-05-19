using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Models.Payment.Response
{
    public class AssignedControlNumber
    {
        public string internal_identifier { get; set; }
        public string control_number { get; set; }
    }
}
