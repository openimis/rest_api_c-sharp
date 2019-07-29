using OpenImis.ModulesV2.CoverageModule.Logic;
using System;

namespace OpenImis.ModulesV2.CoverageModule
{
    public interface ICoverageModule
    {
        ICoverageLogic GetCoverageLogic();
        ICoverageModule SetCoverageLogic(ICoverageLogic coverageLogic);
    }
}
