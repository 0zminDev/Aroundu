using Aroundu.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Application.IntegrationEvents
{
    public class EventCreated : IIntegrationEvent
    {
        public int EventId { get; set; }
        public string Name { get; set; }

        public EventCreated(int eventId, string name)
        {
            EventId = eventId;
            Name = name;
        }
    }
}
