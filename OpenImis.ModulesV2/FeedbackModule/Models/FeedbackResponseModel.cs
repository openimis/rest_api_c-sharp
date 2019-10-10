using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.FeedbackModule.Models
{
    public class FeedbackResponseModel
    {
        public Guid ClaimUUID { get; set; }
        public int? OfficerId { get; set; }
        public string OfficerCode { get; set; }
        public string CHFID { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string HFCode { get; set; }
        public string HFName { get; set; }
        public string ClaimCode { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Phone { get; set; }
        public string FeedbackPromptDate { get; set; }
    }
}
