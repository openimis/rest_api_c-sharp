using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OpenImis.Modules.FeedbackModule.Models;
using OpenImis.Modules.FeedbackModule.Repositories;
using System;
using System.Collections.Generic;

namespace OpenImis.Modules.FeedbackModule.Logic
{
    public class FeedbackLogic: IFeedbackLogic
    {
        private IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        protected IFeedbackRepository feedbackRepository;

        public FeedbackLogic(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            feedbackRepository = new FeedbackRepository(_configuration, _hostingEnvironment);
        }

        public int Post(FeedbackRequest feedbackClaim)
        {
            int response;

            response = feedbackRepository.Post(feedbackClaim);

            return response;
        }

        public List<FeedbackResponseModel> Get(string officerCode)
        {
            List<FeedbackResponseModel> response;

            response = feedbackRepository.Get(officerCode);

            return response;
        }
    }
}
