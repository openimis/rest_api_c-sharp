using OpenImis.Modules.FeedbackModule.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenImis.Modules.FeedbackModule.Repositories
{
    public interface IFeedbackRepository
    {
        List<FeedbackResponseModel> Get(string officerCode);
        int Post(FeedbackRequest feedbackClaim);
    }
}
