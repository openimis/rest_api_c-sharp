using OpenImis.Modules.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Logic
{
    public interface IPolicyLogic
    {
        void SetUserId(int userId);
        DataMessage Enter(Policy model);
        DataMessage Renew(Policy model);
        DataMessage GetCommissions(GetCommissionInputs model);
    }
}
