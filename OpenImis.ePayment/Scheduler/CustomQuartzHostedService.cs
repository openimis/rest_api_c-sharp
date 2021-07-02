using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Scheduler
{
    public class CustomQuartzHostedService : IHostedService
    {

        public IScheduler Scheduler { get; set; }

        private readonly ISchedulerFactory _schedulerFactory;
        private readonly JobMetaData _jobMetaData;
        private readonly IJobFactory _jobFactory;

        public CustomQuartzHostedService(ISchedulerFactory schedulerFactory, JobMetaData jobMetaData, IJobFactory jobFactory)
        {
            _schedulerFactory = schedulerFactory;
            _jobMetaData = jobMetaData;
            _jobFactory = jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler();
            Scheduler.JobFactory = _jobFactory;

            var job = CreateJob(_jobMetaData);
            var trigger = CreateTrigger(_jobMetaData);

            await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            await Scheduler.Start(cancellationToken);

        }
        
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }

        private ITrigger CreateTrigger(JobMetaData jobMetaData)
        {
            return TriggerBuilder.Create()
                .WithIdentity(_jobMetaData.JobId.ToString())
                .WithCronSchedule(_jobMetaData.CronExpression)
                .WithDescription($"{_jobMetaData.JobName}")
                .Build();
        }

        private IJobDetail CreateJob(JobMetaData jobMetaData)
        {
            return JobBuilder
                .Create(_jobMetaData.JobType)
                .WithIdentity(_jobMetaData.JobId.ToString())
                .WithDescription($"{_jobMetaData.JobName}")
                .Build();
        }

    }
}
