using Microsoft.Extensions.Configuration;
using Arch;
using Arch.Bus;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class GamesBusService
    {
        private readonly RabbitMQBus _publisher;

        public GamesBusService(IConfiguration config)
        {
            _publisher = new RabbitMQBus(config["RabbitServer"], "gateway", "game-ex");
            _publisher.InitPublisher();
        }

        public async Task SendEvent(object ev)
        {
            await _publisher.Publish(ev as Contracts.Events.Event);
        }
    }
}