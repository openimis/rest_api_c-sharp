using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.ReportModule.Logic;

namespace OpenImis.ModulesV2.ReportModule
{
    public class ReportModule : IReportModule
    {
        private IConfiguration _configuration;
        private IReportLogic _reportLogic;

        public ReportModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IReportLogic GetReportLogic()
        {
            if (_reportLogic == null)
            {
                _reportLogic = new ReportLogic(_configuration);
            }
            return _reportLogic;
        }

        public IReportModule SetReportLogic(IReportLogic reportLogic)
        {
            _reportLogic = reportLogic;
            return this;
        }
    }
}
