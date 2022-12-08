using Newtonsoft.Json;  
using OpenImis.ModulesV3.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace OpenImis.ModulesV3.ClaimModule.Models
{
    public class ClaimsModel
    {
        [Required]
        public string claim_administrator_code { get; set; }
        public ClaimStatus? status_claim { get; set; }
        [JsonConverter(typeof(IsoDateSerializer), "Invalid visit_date_from")]
        public string insuree_number { get; set; }
        public DateTime? visit_date_from { get; set; }
        [JsonConverter(typeof(IsoDateSerializer), "Invalid visit_date_to")]
        public DateTime? visit_date_to { get; set; }
        [JsonConverter(typeof(IsoDateSerializer), "Invalid processed_date_from")]
        public DateTime? processed_date_from { get; set; }
        [JsonConverter(typeof(IsoDateSerializer), "Invalid processed_date_to")]
        public DateTime? processed_date_to { get; set; }
        [JsonConverter(typeof(IsoDateTimeSerializer), "Invalid last_update_date")]
        public DateTime? last_update_date { get; set; }
    }

    public enum ClaimStatus
    {
        Rejected = 1,
        Entered = 2,
        Checked = 4,
        Processed = 8,
        Valuated = 16
    }
}
