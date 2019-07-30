using OpenImis.ModulesV1.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Logic
{
    public interface IPolicyLogic
    {
        void SetUserId(int userId);
        DataMessage Enter(Policy model);
        DataMessage Renew(Policy model);
        DataMessage GetCommissions(GetCommissionInputs model);
    }
}
