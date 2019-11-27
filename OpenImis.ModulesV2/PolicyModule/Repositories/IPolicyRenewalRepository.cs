using OpenImis.ModulesV2.PolicyModule.Models;
using System;
using System.Collections.Generic;

namespace OpenImis.ModulesV2.PolicyModule.Repositories
{
    public interface IPolicyRenewalRepository
    {
        List<GetPolicyRenewalModel> Get(string officerCode);
        int Post(PolicyRenewalModel policy);
        int Delete(Guid uuid);
        DataMessage GetCommissions(GetCommissionInputs model);
    }
}
