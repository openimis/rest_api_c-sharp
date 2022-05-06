using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenImis.ePayment.Controllers;
using OpenImis.ePayment.Logic;
using OpenImis.ePayment.Models.Payment;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Scheduler
{
    [DisallowConcurrentExecution]
    public class MatchPaymentJob : IJob
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;

        public MatchPaymentJob(IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;
        }

        public Task Execute(IJobExecutionContext context)
        {
            
            var model = new MatchModel
            {
                internal_identifier = 0,
                audit_user_id = 0
            };

            try
            {
                var payment = new PaymentLogic(_configuration, _hostingEnvironment, _loggerFactory);
                var response = payment.MatchPayment(model);

            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
            
            return Task.CompletedTask;
        }
    }
}
