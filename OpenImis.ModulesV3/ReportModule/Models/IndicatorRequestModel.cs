using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.ReportModule.Models
{
    public class IndicatorRequestModel
    {
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime FromDate { get; set; }

        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime ToDate { get; set; }
    }
}
