using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
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
        private IModel _publisherChannel;

        public RabbitMQBus(string server, string queue, string exchange)
            => (_server, _queue, _exchange) = (server, queue, exchange);

        public void InitPublisher()
        {
            Task.Run(() =>
           {
               var factory = new ConnectionFactory()
               {
                   Uri = new Uri(_server)
               };

               using var connection = factory.CreateConnection();
               _publisherChannel = connection.CreateModel();
               _publisherChannel.ExchangeDeclare(_exchange, ExchangeType.Fanout);
               Thread.Sleep(Timeout.Infinite);
           });
        }

        public void Consumer(Action<(string eventType, string body)> callback)
        {
            Task.Run(() =>
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(_server)
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
                    callback(
                        (
                            result.RootElement.GetProperty("Type").GetString(),
                             Encoding.UTF8.GetString(args.Body.Span)
                        )
                    );
                    channel.BasicAck(args.DeliveryTag, false);
                };
                channel.BasicConsume(queue: _queue, autoAck: false, consumer: consumer);

                Thread.Sleep(Timeout.Infinite);

            });
        }

        public async Task Publish(Event eventData)
        {
            var data = JsonSerializer.Serialize(eventData, eventData.GetType());
            var bytes = Encoding.UTF8.GetBytes(data);
            _publisherChannel.BasicPublish(_exchange, "", body: bytes);
            if (_publisherChannel.IsClosed)
            {
                _publisherChannel.Close();
                _publisherChannel.Dispose();
                InitPublisher();
            }
        }
    }
}