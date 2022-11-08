using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.ReportModule.Models
{
    public class SnapshotRequestModel
    {
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime SnapshotDate { get; set; }
    }
}
