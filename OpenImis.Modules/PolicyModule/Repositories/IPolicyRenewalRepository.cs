using OpenImis.Modules.PolicyModule.Models;
using System;
using System.Collections.Generic;

namespace OpenImis.Modules.PolicyModule.Repositories
{
    public interface IPolicyRenewalRepository
    {
        List<GetPolicyRenewalModel> Get(string officerCode);
        int Post(PolicyRenewalModel policy);
        int Delete(Guid uuid);
        DataMessage GetCommissions(GetCommissionInputs model);
    }
}
