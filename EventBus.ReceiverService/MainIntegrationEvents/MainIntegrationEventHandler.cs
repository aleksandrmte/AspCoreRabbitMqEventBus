using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventBus.Common;
using EventBus.Core;
using EventBus.ReceiverService.Services;
using Microsoft.Extensions.Logging;

namespace EventBus.ReceiverService.MainIntegrationEvents
{
    public class MainIntegrationEventHandler: IIntegrationEventHandler<MainIntegrationEvent>
    {
        private readonly ILogger<MainIntegrationEventHandler> _logger;
        private readonly IMockupService _mockupService;

        public MainIntegrationEventHandler(ILogger<MainIntegrationEventHandler> logger, IMockupService mockupService)
        {
            _logger = logger;
            _mockupService = mockupService;
        }

        public Task HandleAsync(MainIntegrationEvent @event)
        {
            _logger.LogInformation($"Event: {@event.Value}");
            _mockupService.ShowMessage(@event.Value);
            return Task.CompletedTask;
        }
    }
}
