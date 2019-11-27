using System;

namespace OpenImis.ModulesV2.ReportModule.Models
{
    public class CumulativeIndicatorsResponseModel
    {
        public int NewPolicies { get; set; }
        public int RenewedPolicies { get; set; }
        public int ExpiredPolicies { get; set; }
        public int SuspendedPolicies { get; set; }
        public double CollectedContribution { get; set; }
    }
}
