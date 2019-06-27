using OpenImis.Modules.CoverageModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.CoverageModule
{
    public interface ICoverageModule
    {
        ICoverageLogic GetCoverageLogic();
    }
}
