
using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.ClaimModule.Models
{
    public class DsiInputModel
    {
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime? last_update_date { get; set; }
    }
}
