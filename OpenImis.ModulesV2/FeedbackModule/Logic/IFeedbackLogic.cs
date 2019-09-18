using OpenImis.ModulesV2.FeedbackModule.Models;
using System;
using System.Collections.Generic;

namespace OpenImis.ModulesV2.FeedbackModule.Logic
{
    public interface IFeedbackLogic
    {
        List<FeedbackModel> Get(string officerCode);
        string GetLoginNameByUserUUID(Guid userUUID);
        int Post(Feedback feedbackClaim);
    }
}
