using OpenImis.ModulesV2.ReportModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV2.ReportModule
{
    public interface IReportModule
    {
        IReportLogic GetReportLogic();
        IReportModule SetReportLogic(IReportLogic reportLogic);
    }
}
