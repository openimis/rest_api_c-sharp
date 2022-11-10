using Newtonsoft.Json;
using OpenImis.ModulesV3.Helpers;
using System;

namespace OpenImis.ModulesV3.PolicyModule.Models
{
    public class GetPolicyRenewalModel
    {
        public int RenewalId { get; set; }
        public int PolicyId { get; set; }
        public int OfficerId { get; set; }
        public string OfficerCode { get; set; }
        public string CHFID { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string VillageName { get; set; }
        
        [JsonConverter(typeof(IsoDateSerializer))]
        public DateTime RenewalPromptDate { get; set; }
        public string Phone { get; set; }
        public Guid RenewalUUID { get; set; }
    }
}
