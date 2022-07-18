using OpenImis.ModulesV1.ClaimModule;
using OpenImis.ModulesV1.CoverageModule;
using OpenImis.ModulesV1.InsureeModule;
using OpenImis.ModulesV1.LoginModule;

namespace OpenImis.ModulesV1
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

    }
}
