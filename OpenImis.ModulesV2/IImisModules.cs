using OpenImis.ModulesV2.LoginModule;
using OpenImis.ModulesV2.ClaimModule;
using OpenImis.ModulesV2.InsureeModule;
using OpenImis.ModulesV2.CoverageModule;
using OpenImis.ModulesV2.PaymentModule;
using OpenImis.ModulesV2.MasterDataModule;
using OpenImis.ModulesV2.SystemModule;
using OpenImis.ModulesV2.FeedbackModule;
using OpenImis.ModulesV2.PolicyModule;
using OpenImis.ModulesV2.ReportModule;
using OpenImis.ModulesV2.PremiumModule;

namespace OpenImis.ModulesV2
{
    /// <summary>
    /// This interface serves to define a Service for the IMIS modules 
    /// </summary>
    public interface IImisModules
    {
        ILoginModule GetLoginModule();

        IClaimModule GetClaimModule();

        IInsureeModule GetInsureeModule();

        ICoverageModule GetCoverageModule();

        IPaymentModule GetPaymentModule();

        IMasterDataModule GetMasterDataModule();

        ISystemModule GetSystemModule();

        IFeedbackModule GetFeedbackModule();

        IPolicyModule GetPolicyModule();

        IReportModule GetReportModule();

        IPremiumModule GetPremiumModule();
    }
}
