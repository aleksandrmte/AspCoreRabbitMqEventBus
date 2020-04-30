using System;
using EventBus.Core;

namespace EventBus.Common
{
    public class MainIntegrationEvent: IIntegrationEvent
    {
        public string Value { get; set; }
    }
}
