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
            var gameConsumerBus = new RabbitMQBus("amqp://guest:guest@localhost:5672", "game", "game-ex");
            var rtmPublisherBus = new RabbitMQBus("amqp://guest:guest@localhost:5672", "rtm", "rtm-ex");
            // TODO: bus publisher for games list service

            Worker.GamesManager = new GamesManager(gameConsumerBus, rtmPublisherBus);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(100);
        }
    }
}
