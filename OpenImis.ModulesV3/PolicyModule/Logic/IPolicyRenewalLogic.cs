using OpenImis.ModulesV3.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.PolicyModule.Logic
{
    public interface IPolicyRenewalLogic
    {
        List<GetPolicyRenewalModel> Get(string officerCode);
        int Post(PolicyRenewalModel policy);
        int Delete(Guid uuid);
        DataMessage GetCommissions(GetCommissionInputs model);
    }
}
