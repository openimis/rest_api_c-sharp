using OpenImis.Modules.CoverageModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.CoverageModule.Logic
{
    public interface ICoverageLogic
    {
        CoverageModel Get(string insureeNumber);
    }
}
