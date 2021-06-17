using OpenImis.ModulesV3.ReportModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.ReportModule
{
    public interface IReportModule
    {
        IReportLogic GetReportLogic();
        IReportModule SetReportLogic(IReportLogic reportLogic);
    }
}
