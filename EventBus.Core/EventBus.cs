using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventBus.Core
{
    public class EventBus: IEventBus
    {
        private readonly IConnection _connection;

        private IModel _channel;

        public IModel Channel
        {
            get
            {
                if (_channel == null)
                    _channel = _connection.CreateModel();

                return _channel;
            }
        }

        private readonly IServiceProvider _serviceProvider;

        public EventBus(IConfiguration config, IServiceProvider serviceProvider)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = config["EventBus:HostName"],
                Port = int.Parse(config["EventBus:Port"]),
                UserName = config["EventBus:UserName"],
                Password = config["EventBus:Password"],
                DispatchConsumersAsync = true
            };
            _connection = connectionFactory.CreateConnection();

            _serviceProvider = serviceProvider;
        }

        public void Subscribe<TH, TE>(string exchangeName, string subscriberName) where TH : IIntegrationEventHandler<TE> where TE : IIntegrationEvent
        {
            BindQueue(exchangeName, subscriberName);

            var consumer = new AsyncEventingBasicConsumer(Channel);

            consumer.Received += async (obj, args) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandler<TE>>();
                var jsonMessage = Encoding.UTF8.GetString(args.Body);
                var message = JsonConvert.DeserializeObject<TE>(jsonMessage);
                await handler.HandleAsync(message);
                Channel.BasicAck(args.DeliveryTag, false);
            };

            Channel.BasicConsume(subscriberName, false, consumer);
        }

        public void Publish(IIntegrationEvent @event, string exchangeName)
        {
            CreateExchangeIfNotExists(exchangeName);

            var jsonEvent = JsonConvert.SerializeObject(@event);
            var bytesEvent = Encoding.UTF8.GetBytes(jsonEvent);

            Channel.BasicPublish(exchangeName, string.Empty, body: bytesEvent);
        }

        private void CreateExchangeIfNotExists(string exchangeName)
        {
            Channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);
        }

        private void BindQueue(string exchangeName, string subscriberName)
        {
            CreateExchangeIfNotExists(exchangeName);

            Channel.QueueDeclare(subscriberName, true, false, autoDelete: false);
            Channel.QueueBind(subscriberName, exchangeName, string.Empty);
        }
    }
}
