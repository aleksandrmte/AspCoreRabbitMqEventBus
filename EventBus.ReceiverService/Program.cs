using System;
using EventBus.Common;
using EventBus.Core;
using EventBus.ReceiverService.MainIntegrationEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.ReceiverService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var serviceProvider = ConfigureServices.Configure();

            var eventBus = serviceProvider.GetRequiredService<IEventBus>();
            eventBus.Subscribe<MainIntegrationEventHandler, MainIntegrationEvent>(nameof(MainIntegrationEvent),
                AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
