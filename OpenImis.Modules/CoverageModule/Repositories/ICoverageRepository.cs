using OpenImis.Modules.CoverageModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.CoverageModule.Repositories
{
    public interface ICoverageRepository
    {
        CoverageModel Get(string insureeNumber);
    }
}
