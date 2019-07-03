using OpenImis.Modules.InsureeModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule
{
    public interface IInsureeModule
    {
        IFamilyLogic GetFamilyLogic();
        IContributionLogic GetContributionLogic();
        IPolicyLogic GetPolicyLogic();
    }
}
