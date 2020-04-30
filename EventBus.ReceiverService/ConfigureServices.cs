using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EventBus.Common;
using EventBus.Core;
using EventBus.ReceiverService.MainIntegrationEvents;
using EventBus.ReceiverService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.ReceiverService
{
    public class ConfigureServices
    {
        public static ServiceProvider Configure()
        {
            //setup our DI
            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IEventBus, Core.EventBus>()
                .AddScoped<IMockupService, MockupService>()
                .AddTransient<IIntegrationEventHandler<MainIntegrationEvent>, MainIntegrationEventHandler>();

            services.Add(new ServiceDescriptor(typeof(IConfiguration),
                provider => new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json",
                        optional: false,
                        reloadOnChange: true)
                    .Build(),
                ServiceLifetime.Singleton));

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        } 
    }
}
