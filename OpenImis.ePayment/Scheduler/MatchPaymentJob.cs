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

        public MatchPaymentJob(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
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
                var payment = new PaymentLogic(_configuration, _hostingEnvironment);
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
