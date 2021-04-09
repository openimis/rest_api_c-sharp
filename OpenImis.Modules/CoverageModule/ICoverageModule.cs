using OpenImis.Modules.CoverageModule.Logic;
using System;

namespace OpenImis.Modules.CoverageModule
{
    public interface ICoverageModule
    {
        ICoverageLogic GetCoverageLogic();
        ICoverageModule SetCoverageLogic(ICoverageLogic coverageLogic);
    }
}
