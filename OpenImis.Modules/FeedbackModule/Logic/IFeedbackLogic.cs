using OpenImis.Modules.FeedbackModule.Models;
using System;
using System.Collections.Generic;

namespace OpenImis.Modules.FeedbackModule.Logic
{
    public interface IFeedbackLogic
    {
        List<FeedbackResponseModel> Get(string officerCode);
        int Post(FeedbackRequest feedbackClaim);
    }
}
