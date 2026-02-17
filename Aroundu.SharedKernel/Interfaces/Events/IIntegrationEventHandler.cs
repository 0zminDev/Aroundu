namespace Aroundu.SharedKernel.Interfaces.Events
{
    public interface IIntegrationEventHandler<in TEvent> : IDependency
        where TEvent : IIntegrationEvent
    {
        Task Handle(TEvent @event, CancellationToken ct);
    }
}
