using OpenImis.Modules.ReportModule.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.ReportModule
{
    public interface IReportModule
    {
        IReportLogic GetReportLogic();
        IReportModule SetReportLogic(IReportLogic reportLogic);
    }
}
