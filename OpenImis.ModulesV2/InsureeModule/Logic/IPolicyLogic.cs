using OpenImis.ModulesV2.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.InsureeModule.Logic
{
    public interface IPolicyLogic
    {
        List<GetPolicyModel> Get(string officerCode);
        int Post(PolicyRenewalModel policy);
        int Delete(Guid uuid);
        string GetLoginNameByUserUUID(Guid userUUID);
    }
}
