using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    public class BulkControlNumbers
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int ProdId { get; set; }
        public int OfficerId { get; set; }
        public string ControlNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime DateReceived { get; set; }
        public int? FamilyId { get; set; }
    }
}
