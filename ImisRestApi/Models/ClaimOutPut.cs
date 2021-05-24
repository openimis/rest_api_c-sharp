using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public class ClaimOutPut
    {
        public string claim_uuid { get; set; }
        public string health_facility_code { get; set; }
        public string health_facility_name { get; set; }
        public string insurance_number { get; set; }
        public string patient_name { get; set; }
        public string main_dg { get; set; }
        public string claim_number { get; set; }
        public string date_claimed { get; set; }
        public string visit_date_from { get; set; }
        public string visit_type { get; set; }
        public string claim_status { get; set; }
        public string sec_dg_1 { get; set; }
        public string sec_dg_2 { get; set; }
        public string sec_dg_3 { get; set; }
        public string sec_dg_4 { get; set; }
        public string visit_date_to { get; set; }
        public decimal? claimed { get; set; }
        public decimal? approved { get; set; }
        public decimal? adjusted { get; set; }
        public string explanation { get; set; }
        public string adjustment { get; set; }
        public string guarantee_number { get; set; }
        public List<ClaimService> services { get; set; }
        public List<ClaimItem> items { get; set; }
    }

    public class ClaimItem
    {
        public string claim_uuid { get; set; }
        public string claim_number { get; set; }
        public string item { get; set; }
        public string item_code { get; set; }
        public decimal? item_qty { get; set; }
        public decimal? item_price { get; set; }
        public decimal? item_adjusted_qty { get; set; }
        public decimal? item_adjusted_price { get; set; }
        public string item_explination { get; set; }
        public string item_justificaion { get; set; }
        public decimal? item_valuated { get; set; }
        public string item_result { get; set; }
    }

    public class ClaimService
    {
        public string claim_uuid { get; set; }
        public string claim_number { get; set; }
        public string service { get; set; }
        public string service_code { get; set; }
        public decimal? service_qty { get; set; }
        public decimal? service_price { get; set; }
        public decimal? service_adjusted_qty { get; set; }
        public decimal? service_adjusted_price { get; set; }
        public string service_explination { get; set; }
        public string service_justificaion { get; set; }
        public decimal? service_valuated { get; set; }
        public string service_result { get; set; }
    }

}
