using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnchainedBackend.Controllers;

namespace UnchainedBackend.Helpers
{
    public class AuctionTask : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<MintingController> _logger;
        private Timer _timer;
        private int _auctionId;

        public AuctionTask(ILogger<MintingController> logger, int auctionId)
        {
            _logger = logger;
            _auctionId = auctionId;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("STARTED AUCTION " + _auctionId + " " + DateTime.Now);

            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(10),
                TimeSpan.Zero);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            // TODO: AuctionEnd here
            _logger.LogInformation("Auction now ENDED" + DateTime.Now);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
