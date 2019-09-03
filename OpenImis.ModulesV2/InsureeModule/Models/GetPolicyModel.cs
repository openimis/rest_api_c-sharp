using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Models
{
    public class GetPolicyModel
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
        public string RenewalPromptDate { get; set; }
        public string Phone { get; set; }
    }
}
