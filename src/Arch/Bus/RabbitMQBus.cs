using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Arch.Bus
{
    public class RabbitMQBus : IBus
    {
        private string _server;
        private string _queue;
        private string _exchange;

        public RabbitMQBus(string server, string queue, string exchange)
            => (_server, _queue, _exchange) = (server, queue, exchange);

        public void Consumer(Action<(string eventType, string body)> callback)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _server
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(_exchange, ExchangeType.Fanout, false, false, null);
            channel.QueueDeclare(_queue, false, false, false, null);
            channel.QueueBind(_queue, _exchange, "", null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                using var result = JsonDocument.Parse(args.Body);
                callback((result.RootElement.GetProperty("Type").GetString(), args.Body.ToString()));
                channel.BasicAck(args.DeliveryTag, false);
            };
            channel.BasicConsume(queue: _queue, autoAck: false, consumer: consumer);
        }

        public Task Publish(Event Event)
        {
            throw new NotImplementedException();
        }
    }
}