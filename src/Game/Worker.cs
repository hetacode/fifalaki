using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arch.Bus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Game
{
    public class Worker : BackgroundService
    {
        public static GamesManager GamesManager;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            var bus = new RabbitMQBus("amqp://guest:guest@localhost:5672", "game", "game");
            Worker.GamesManager = new GamesManager(bus);
        }

        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
