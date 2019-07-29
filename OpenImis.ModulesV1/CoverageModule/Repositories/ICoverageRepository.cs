using OpenImis.ModulesV1.CoverageModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.CoverageModule.Repositories
{
    public interface ICoverageRepository
    {
        CoverageModel Get(string insureeNumber);
    }
}
