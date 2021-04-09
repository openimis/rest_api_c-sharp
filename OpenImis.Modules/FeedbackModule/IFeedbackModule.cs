using OpenImis.Modules.FeedbackModule.Logic;

namespace OpenImis.Modules.FeedbackModule
{
    public interface IFeedbackModule
    {
        IFeedbackLogic GetFeedbackLogic();
        IFeedbackModule SetFeedbackLogic(IFeedbackLogic feedbackLogic);
    }
}
