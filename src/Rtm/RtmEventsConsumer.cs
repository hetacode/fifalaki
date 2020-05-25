using System;
using System.Text.Json;
using Arch;
using Arch.Bus;
using Microsoft.Extensions.Configuration;
using Rtm.Hubs;

namespace Rtm
{
    public class RtmEventsConsumer
    {
        private readonly GameHub _hub;

        public RtmEventsConsumer(IConfiguration config, GameHub hub)
        {
            _hub = hub;
            var busConsumer = new RabbitMQBus(config["RabbitServer"], "rtm", "rtm-ex");
            busConsumer.Consumer(ConsumeEvents);
        }

        private void ConsumeEvents((string eventType, string body) data)
        {
            Console.WriteLine($"event: {data.body}");

            var eventType = EventRecognizer.GetEventBy(data.eventType);
            var e = JsonSerializer.Deserialize(data.body, eventType);
        }
    }
}