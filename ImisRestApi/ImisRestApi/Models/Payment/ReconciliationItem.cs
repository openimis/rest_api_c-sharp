using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models.Payment
{
    public class ReconciliationItem
    {
        public string control_number { get; set; }
        public string insurance_number { get; set; }
        public string enrolment_officer_code { get; set; }
        public string transaction_identification { get; set; }
        public string receipt_identification { get; set; }
        public double received_amount { get; set; }
        public string received_date { get; set; }
        public string payment_date { get; set; }
        public string payment_origin { get; set; }
        public string type_of_payment { get; set; }
        public string language { get; set; }
    }
}
