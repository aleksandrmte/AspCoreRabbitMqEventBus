using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Common;
using EventBus.Core;
using Microsoft.AspNetCore.Mvc;

namespace EventBus.MainService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public MainController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpPost]
        public void SendMessageAsync([FromBody] MainIntegrationEvent message)
        {
            _eventBus.Publish(message, nameof(MainIntegrationEvent));
        }
    }
}
