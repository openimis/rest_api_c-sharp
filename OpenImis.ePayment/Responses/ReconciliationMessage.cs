using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Responses
{
    public class ReconciliationMessage
    {
        public bool error_occurred { get; set; }
        public object transactions { get; set; }
    }
}
