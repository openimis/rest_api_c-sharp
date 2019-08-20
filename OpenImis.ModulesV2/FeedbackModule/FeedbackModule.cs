using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.FeedbackModule.Logic;

namespace OpenImis.ModulesV2.FeedbackModule
{
    public class FeedbackModule : IFeedbackModule
    {
        private IConfiguration _configuration;
        private IFeedbackLogic _feedbackLogic;

        public FeedbackModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IFeedbackLogic GetFeedbackLogic()
        {
            if (_feedbackLogic == null)
            {
                _feedbackLogic = new FeedbackLogic(_configuration);
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
