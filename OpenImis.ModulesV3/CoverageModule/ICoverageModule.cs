using OpenImis.ModulesV3.CoverageModule.Logic;
using System;

namespace OpenImis.ModulesV3.CoverageModule
{
    public interface ICoverageModule
    {
        ICoverageLogic GetCoverageLogic();
        ICoverageModule SetCoverageLogic(ICoverageLogic coverageLogic);
    }
}
