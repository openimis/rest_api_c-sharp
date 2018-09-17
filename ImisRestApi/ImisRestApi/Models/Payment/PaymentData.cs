using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Payment
{
    public class PaymentData
    {
        public string PaymentId { get; set; }
        public string ControlNumber { get; set; }
        public string ProductCode { get; set; }
        public string EnrolmentOfficerCode { get; set; }
        public string TransactionId { get; set; }
        public double ReceivedAmount { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentOrigin { get; set; }
        public bool ErrorOccured { get; set; }
    }
}
