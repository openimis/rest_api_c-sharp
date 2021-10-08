using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.ClaimModule.Models
{
    public class SubmitClaimResponse
    {
        public string ClaimCode { get; set; }
        public int Response { get; set; }
        public string Message { get; set; }
    }

}
