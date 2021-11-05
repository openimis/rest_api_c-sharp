using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.ClaimModule.Models
{
    public class PaymentLists
    {
        public DateTime update_since_last { get; set; }
        public string health_facility_code { get; set; }
        public string health_facility_name { get; set; }
        public List<CodeNamePrice> pricelist_services { get; set; }
        public List<CodeNamePrice> pricelist_items { get; set; }
    }
}
