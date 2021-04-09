using OpenImis.Modules.PolicyModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PolicyModule
{
    public interface IPolicyModule
    {
        IPolicyRenewalLogic GetPolicyRenewalLogic();
        IPolicyModule SetPolicyLogic(IPolicyRenewalLogic policyRenewalLogic);
    }
}
