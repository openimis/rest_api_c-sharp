using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    public class BulkControlNumbersForEO
    {
        public ControlNumbersForEOHeader Header { get; set; }
        public List<ControlNumbersForEO> ControlNumbers { get; set; }
    }

    public class ControlNumbersForEOHeader
    {
        public int Error { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ControlNumbersForEO
    {
        public int ControlNumberId { get; set; }
        public int BillId { get; set; }
        public string ProductCode { get; set; }
        public string OfficerCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ControlNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
