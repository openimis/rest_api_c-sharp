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

        public FeedbackModule FeedbackModule { get; set; }
        public PremiumModule PremiumModule { get; set; }
        public SystemModule SystemModule { get; set; }
        public MasterDataModule MasterDataModule { get; set; }
        public ReportModule ReportModule { get; set; }
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

    public class FeedbackModule
    {
        public string FeedbackLogic { get; set; }
    }

    public class PremiumModule
    {
        public string PremiumLogic { get; set; }
    }

    public class SystemModule
    {
        public string SystemLogic { get; set; }
    }

    public class MasterDataModule
    {
        public string MasterDataLogic { get; set; }
    }

    public class ReportModule
    {
        public string ReportLogic { get; set; }
    }
}
