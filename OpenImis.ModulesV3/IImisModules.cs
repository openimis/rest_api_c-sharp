using OpenImis.Security;
using OpenImis.ModulesV3.ClaimModule;
using OpenImis.ModulesV3.InsureeModule;
using OpenImis.ModulesV3.CoverageModule;
using OpenImis.ModulesV3.MasterDataModule;
using OpenImis.ModulesV3.SystemModule;
using OpenImis.ModulesV3.FeedbackModule;
using OpenImis.ModulesV3.PolicyModule;
using OpenImis.ModulesV3.ReportModule;
using OpenImis.ModulesV3.PremiumModule;

namespace OpenImis.ModulesV3
{
    /// <summary>
    /// This interface serves to define a Service for the IMIS modules 
    /// </summary>
    public interface IImisModules
    {
        
        ClaimModule.ClaimModule GetClaimModule();

        IInsureeModule GetInsureeModule();

        ICoverageModule GetCoverageModule();

        IMasterDataModule GetMasterDataModule();

        ISystemModule GetSystemModule();

        IFeedbackModule GetFeedbackModule();

        IPolicyModule GetPolicyModule();

        IReportModule GetReportModule();

        IPremiumModule GetPremiumModule();
    }
}
