using System;

namespace OpenImis.ModulesV2.ReportModule.Models
{
    public class ReportRequestModel
    {
        public string OfficerCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
