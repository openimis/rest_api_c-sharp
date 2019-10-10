using OpenImis.ModulesV2.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.PolicyModule.Logic
{
    public interface IPolicyRenewalLogic
    {
        List<GetPolicyRenewalModel> Get(string officerCode);
        int Post(PolicyRenewalModel policy);
        int Delete(Guid uuid);
        string GetLoginNameByUserUUID(Guid userUUID);
    }
}
