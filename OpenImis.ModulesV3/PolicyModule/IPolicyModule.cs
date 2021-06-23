using OpenImis.ModulesV3.PolicyModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.PolicyModule
{
    public interface IPolicyModule
    {
        IPolicyRenewalLogic GetPolicyRenewalLogic();
        IPolicyModule SetPolicyLogic(IPolicyRenewalLogic policyRenewalLogic);
    }
}
