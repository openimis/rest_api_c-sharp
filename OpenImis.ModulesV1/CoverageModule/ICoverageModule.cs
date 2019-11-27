using OpenImis.ModulesV1.CoverageModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV1.CoverageModule
{
    public interface ICoverageModule
    {
        ICoverageLogic GetCoverageLogic();
        ICoverageModule SetCoverageLogic(ICoverageLogic coverageLogic);
    }
}
