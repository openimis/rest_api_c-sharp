using OpenImis.ModulesV1.InsureeModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule
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
