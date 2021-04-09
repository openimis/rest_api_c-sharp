using OpenImis.Modules.LoginModule;
using OpenImis.Modules.ClaimModule;
using OpenImis.Modules.InsureeModule;
using OpenImis.Modules.CoverageModule;
using OpenImis.Modules.PaymentModule;
using OpenImis.Modules.MasterDataModule;
using OpenImis.Modules.SystemModule;
using OpenImis.Modules.FeedbackModule;
using OpenImis.Modules.PolicyModule;
using OpenImis.Modules.ReportModule;
using OpenImis.Modules.PremiumModule;

namespace OpenImis.Modules
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
