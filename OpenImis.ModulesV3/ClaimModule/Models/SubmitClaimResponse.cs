using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.ClaimModule.Models
{
    public class RejectedItem
    {
        public string Code { get; set; }
        public short? Error { get; set; }
        public string Reason { get; set; }
    }

    public class RejectedService
    {
        public string Code { get; set; }
        public short? Error { get; set; }
        public string Reason { get; set; }
    }

    public class SubmitClaimResponse
    {
        public string ClaimCode { get; set; }
        public int Response { get; set; }
        public string Message { get; set; }

        public List<RejectedItem> RejectedItems { get; set; }

        public List<RejectedService> RejectedServices { get; set; }

    }

}
