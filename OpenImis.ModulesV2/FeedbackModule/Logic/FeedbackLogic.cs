using Microsoft.Extensions.Configuration;

namespace OpenImis.ModulesV2.FeedbackModule.Logic
{
    public class FeedbackLogic: IFeedbackLogic
    {
        private IConfiguration _configuration;

        public FeedbackLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
