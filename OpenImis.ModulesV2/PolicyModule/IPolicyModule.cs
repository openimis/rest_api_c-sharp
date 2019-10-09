using OpenImis.ModulesV2.PolicyModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.PolicyModule
{
    public interface IPolicyModule
    {
        IPolicyRenewalLogic GetPolicyRenewalLogic();
        IPolicyModule SetPolicyLogic(IPolicyRenewalLogic policyRenewalLogic);
    }
}
