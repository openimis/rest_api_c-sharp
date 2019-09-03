using Microsoft.Extensions.Configuration;
using OpenImis.ModulesV2.FeedbackModule.Models;
using OpenImis.ModulesV2.FeedbackModule.Repositories;
using System;
using System.Collections.Generic;

namespace OpenImis.ModulesV2.FeedbackModule.Logic
{
    public class FeedbackLogic: IFeedbackLogic
    {
        private IConfiguration _configuration;

        protected IFeedbackRepository feedbackRepository;

        public FeedbackLogic(IConfiguration configuration)
        {
            _configuration = configuration;

            feedbackRepository = new FeedbackRepository(_configuration);
        }

        public List<FeedbackModel> Get(string officerCode)
        {
            List<FeedbackModel> response;

            response = feedbackRepository.Get(officerCode);

            return response;
        }

        public string GetLoginNameByUserUUID(Guid userUUID)
        {
            string response;

            response = feedbackRepository.GetLoginNameByUserUUID(userUUID);

            return response;
        }
    }
}
