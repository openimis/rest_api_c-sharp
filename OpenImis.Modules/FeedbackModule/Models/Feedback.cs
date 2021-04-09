using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.FeedbackModule.Models
{
    public class Feedback
    {
        public string Officer { get; set; }
        public int ClaimID { get; set; }
        public string CHFID { get; set; }
        public string Answers { get; set; }
        public string Date { get; set; }
    }
}
