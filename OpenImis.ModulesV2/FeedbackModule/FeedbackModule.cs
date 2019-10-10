using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.FeedbackModule.Logic;

namespace OpenImis.ModulesV2.FeedbackModule
{
    public class FeedbackModule : IFeedbackModule
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        private IFeedbackLogic _feedbackLogic;

        public FeedbackModule(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IFeedbackLogic GetFeedbackLogic()
        {
            if (_feedbackLogic == null)
            {
                _feedbackLogic = new FeedbackLogic(_configuration, _hostingEnvironment);
            }
            return _feedbackLogic;
        }

        public IFeedbackModule SetFeedbackLogic(IFeedbackLogic feedbackLogic)
        {
            _feedbackLogic = feedbackLogic;
            return this;
        }
    }
}
