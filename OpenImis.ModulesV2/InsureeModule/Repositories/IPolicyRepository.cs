using OpenImis.ModulesV2.InsureeModule.Models;
using System;
using System.Collections.Generic;

namespace OpenImis.ModulesV2.InsureeModule.Repositories
{
    public interface IPolicyRepository
    {
        List<GetPolicyModel> Get(string officerCode);
        int Post(PolicyRenewalModel policy);
        int Delete(Guid uuid);
    }
}
