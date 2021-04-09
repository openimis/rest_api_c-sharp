using OpenImis.Modules.InsureeModule.Logic;

namespace OpenImis.Modules.InsureeModule
{
    public interface IInsureeModule
    {
        IFamilyLogic GetFamilyLogic();
        IContributionLogic GetContributionLogic();
        IInsureeLogic GetInsureeLogic();
        IInsureeModule SetFamilyLogic(IFamilyLogic familyLogic);
        IInsureeModule SetContributionLogic(IContributionLogic contributionLogic);
        IInsureeModule SetInsureeLogic(IInsureeLogic insureeLogic);
    }
}
