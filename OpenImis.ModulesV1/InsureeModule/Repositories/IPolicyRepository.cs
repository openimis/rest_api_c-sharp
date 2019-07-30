using OpenImis.ModulesV1.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Repositories
{
    public interface IPolicyRepository
    {
        DataMessage Enter(Policy model);
        DataMessage Renew(Policy model);
        DataMessage GetCommissions(GetCommissionInputs model);
        int UserId { get; set; }
    }
}
