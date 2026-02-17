using System.Diagnostics.CodeAnalysis;
using Aroundu.SharedKernel.Interfaces;

namespace Aroundu.SharedKernel.IntegrationEvents
{
    public class EventCreatedEvent : IIntegrationEvent
    {
        public required int EventId { get; set; }
        public required string Name { get; set; }

        public EventCreatedEvent() { }
        
        [SetsRequiredMembers]
        public EventCreatedEvent(int eventId, string name)
        {
            EventId = eventId;
            Name = name;
        }
    }
}
