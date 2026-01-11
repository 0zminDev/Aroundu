using Aroundu.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.SharedKernel.IntegrationEvents
{
    public class EventCreatedEvent : IIntegrationEvent
    {
        public int EventId { get; set; }
        public string Name { get; set; }

        public EventCreatedEvent() { }
        public EventCreatedEvent(int eventId, string name)
        {
            EventId = eventId;
            Name = name;
        }
    }
}
