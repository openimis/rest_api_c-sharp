using OpenImis.ModulesV2.FeedbackModule.Models;
using System;
using System.Collections.Generic;

namespace OpenImis.ModulesV2.FeedbackModule.Logic
{
    public interface IFeedbackLogic
    {
        List<FeedbackResponseModel> Get(string officerCode);
        int Post(FeedbackRequest feedbackClaim);
    }
}
