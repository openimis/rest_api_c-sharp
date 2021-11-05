using Newtonsoft.Json;
using OpenImis.ModulesV3.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.FeedbackModule.Models
{
    public class FeedbackRequest
    {
        public string Officer { get; set; }
        public Guid ClaimUUID { get; set; }
        public string CHFID { get; set; }
        public string Answers { get; set; }

        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime Date { get; set; }
    }
}
