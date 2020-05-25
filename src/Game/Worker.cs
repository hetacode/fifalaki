using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arch.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Game
{
    public class Worker : BackgroundService
    {
        public static GamesManager GamesManager;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            
            var busServer = configuration["RabbitServer"];

            var gameConsumerBus = new RabbitMQBus(busServer, "game", "game-ex");
            var rtmPublisherBus = new RabbitMQBus(busServer, "rtm", "rtm-ex");
            // TODO: bus publisher for games list service

            Worker.GamesManager = new GamesManager(gameConsumerBus, rtmPublisherBus);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(100);
        }
    }
}
