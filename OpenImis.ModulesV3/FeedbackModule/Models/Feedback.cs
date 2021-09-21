using Newtonsoft.Json;
using OpenImis.ModulesV3.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.FeedbackModule.Models
{
    public class Feedback
    {
        public string Officer { get; set; }
        public int ClaimID { get; set; }
        public string CHFID { get; set; }
        public string Answers { get; set; }

        [JsonConverter(typeof(IsoDateOnlyDatetimeSerializer))]
        public DateTime Date { get; set; }
    }
}
