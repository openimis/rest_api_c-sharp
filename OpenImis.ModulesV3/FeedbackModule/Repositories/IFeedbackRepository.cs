using OpenImis.ModulesV3.FeedbackModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.ModulesV3.FeedbackModule.Repositories
{
    public interface IFeedbackRepository
    {
        List<FeedbackResponseModel> Get(string officerCode);
        int Post(FeedbackRequest feedbackClaim);
    }
}
