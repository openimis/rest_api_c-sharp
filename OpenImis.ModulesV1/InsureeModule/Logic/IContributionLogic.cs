using OpenImis.ModulesV1.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Logic
{
    public interface IContributionLogic
    {
        DataMessage Enter(Contribution model);
        void SetUserId(int userId);
    }
}
