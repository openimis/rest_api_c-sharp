﻿using OpenImis.ModulesV3.PolicyModule.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenImis.ModulesV3.PolicyModule.Repositories
{
    public interface IPolicyRenewalRepository
    {
        List<GetPolicyRenewalModel> Get(string officerCode);
        int Post(PolicyRenewalModel policy);
        int Delete(Guid uuid);
        DataMessage GetCommissions(GetCommissionInputs model);
        Task<DataMessage> SelfRenewal(SelfRenewal renewal);
    }
}
