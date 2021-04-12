using OpenImis.Modules.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.PolicyModule.Logic
{
    public interface IPolicyRenewalLogic
    {
        List<GetPolicyRenewalModel> Get(string officerCode);
        int Post(PolicyRenewalModel policy);
        int Delete(Guid uuid);
        DataMessage GetCommissions(GetCommissionInputsMv model);
    }
}
