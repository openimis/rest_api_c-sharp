using OpenImis.Modules.InsureeModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.InsureeModule.Repositories
{
    public interface IContributionRepository
    {
        DataMessage Enter(Contribution model);
        int UserId { get; set; }
    }
}
