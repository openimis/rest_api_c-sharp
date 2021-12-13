using OpenImis.ModulesV3.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenImis.ModulesV3.PolicyModule.Logic
{
    public interface IPolicyRenewalLogic
    {
        List<GetPolicyRenewalModel> Get(string officerCode);
        int Post(PolicyRenewalModel policy);
        int Delete(Guid uuid);
        DataMessage GetCommissions(GetCommissionInputs model);
        Task<DataMessage> SelfRenewal(SelfRenewal renewal);
    }
}
