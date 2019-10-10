using OpenImis.ModulesV2.InsureeModule.Logic;

namespace OpenImis.ModulesV2.InsureeModule
{
    public interface IInsureeModule
    {
        IFamilyLogic GetFamilyLogic();
        IContributionLogic GetContributionLogic();
        IInsureeModule SetFamilyLogic(IFamilyLogic familyLogic);
        IInsureeModule SetContributionLogic(IContributionLogic contributionLogic);
    }
}
