using Aroundu.SharedKernel.Interfaces;

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
