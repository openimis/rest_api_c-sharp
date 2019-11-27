using OpenImis.ModulesV1.CoverageModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.CoverageModule.Logic
{
    public interface ICoverageLogic
    {
        CoverageModel Get(string insureeNumber);
    }
}
