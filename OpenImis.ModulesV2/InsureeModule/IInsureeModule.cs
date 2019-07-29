using OpenImis.ModulesV2.InsureeModule.Logic;
using System;

namespace OpenImis.ModulesV2.InsureeModule
{
    public interface IInsureeModule
    {
        IFamilyLogic GetFamilyLogic();
        IContributionLogic GetContributionLogic();
        IPolicyLogic GetPolicyLogic();
        IInsureeModule SetFamilyLogic(IFamilyLogic familyLogic);
        IInsureeModule SetPolicyLogic(IPolicyLogic policyLogic);
        IInsureeModule SetContributionLogic(IContributionLogic contributionLogic);
    }
}
