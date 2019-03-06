using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public class DiagnosisServiceItem
    {
        public List<CodeName> diagnoses { get; set; }
        public List<CodeNamePrice> services { get; set; }
        public List<CodeNamePrice> items { get; set; }
        public DateTime update_since_last { get; set; }
    }

    public class CodeName
    {
        public string code { get; set; }
        public string name { get; set; }
    }

    public class PaymentLists
    {
        public DateTime update_since_last { get; set; }
        public string health_facility_code { get; set; }
        public string health_facility_name { get; set; }
        public List<CodeNamePrice> pricelist_services { get; set; }
        public List<CodeNamePrice> pricelist_items { get; set; }
    }

    public class CodeNamePrice
    {
        public string code { get; set; }
        public string name { get; set; }
        public string price { get; set; }
    }
}
