using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.ReceiverService.Services
{
    public class MockupService: IMockupService
    {
        public void ShowMessage(string text)
        {
            Console.WriteLine($"Message from main service: {text}");
        }
    }
}
