using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Core
{
    public interface IEventBus
    {
        void Subscribe<TH, TE>(string exchangeName, string subscriberName) 
            where TH: IIntegrationEventHandler<TE> 
            where TE: IIntegrationEvent;

        void Publish(IIntegrationEvent @event, string exchangeName);
    }
}
