using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.FeedbackModule.Models
{
    public class Feedback
    {
        public string Officer { get; set; }
        public int ClaimID { get; set; }
        public string CHFID { get; set; }
        public string Answers { get; set; }

        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime Date { get; set; }
    }
}
