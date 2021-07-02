using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Scheduler
{
    public class CustomQuartzJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomQuartzJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;
            return (IJob)_serviceProvider.GetService(jobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            
        }
    }
}
