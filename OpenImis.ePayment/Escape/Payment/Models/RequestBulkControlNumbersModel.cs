using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    public class RequestBulkControlNumbersModel
    {
        public int ControlNumberCount { get; set; }
        public string ProductCode { get; set; }

    }
}
