using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.Helpers
{
    public class ConfigImisModules
    {
        public string Version { get; set; }
        public LoginModule LoginModule { get; set; }
        public ClaimModule ClaimModule { get; set; }
        public CoverageModule CoverageModule { get; set; }
        public InsureeModule InsureeModule { get; set; }
        public PaymentModule PaymentModule { get; set; }
    }

    public class LoginModule
    {
        public string LoginLogic { get; set; }
    }

    public class ClaimModule
    {
        public string ClaimLogic { get; set; }
    }

    public class CoverageModule
    {
        public string CoverageLogic { get; set; }
    }

    public class InsureeModule
    {
        public string FamilyLogic { get; set; }
        public string PolicyLogic { get; set; }
        public string ContributionLogic { get; set; }
    }

    public class PaymentModule
    {
        public string PaymentLogic { get; set; }
    }
}
