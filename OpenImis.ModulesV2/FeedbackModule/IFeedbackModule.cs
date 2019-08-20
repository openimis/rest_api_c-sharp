using OpenImis.ModulesV2.FeedbackModule.Logic;

namespace OpenImis.ModulesV2.FeedbackModule
{
    public interface IFeedbackModule
    {
        IFeedbackLogic GetFeedbackLogic();
        IFeedbackModule SetFeedbackLogic(IFeedbackLogic feedbackLogic);
    }
}
