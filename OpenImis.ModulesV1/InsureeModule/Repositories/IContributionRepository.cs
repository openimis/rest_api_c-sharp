using OpenImis.ModulesV1.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.InsureeModule.Repositories
{
    public interface IContributionRepository
    {
        DataMessage Enter(Contribution model);
        int UserId { get; set; }
    }
}
