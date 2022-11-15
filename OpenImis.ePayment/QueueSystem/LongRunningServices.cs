using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenImis.ePayment.QueueSystem
{
    public class LongRunningServices : BackgroundService
    {
        private readonly BackgroundWorkerQueue _queue;

        public LongRunningServices(BackgroundWorkerQueue queue)
        {
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _queue.DequeueAsync(stoppingToken);
                await workItem(stoppingToken);
            }
        }
    }
}
