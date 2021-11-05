using OpenImis.ModulesV3.FeedbackModule.Logic;

namespace OpenImis.ModulesV3.FeedbackModule
{
    public interface IFeedbackModule
    {
        IFeedbackLogic GetFeedbackLogic();
        IFeedbackModule SetFeedbackLogic(IFeedbackLogic feedbackLogic);
    }
}
