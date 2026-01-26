namespace Aroundu.SharedKernel.Interfaces
{
    public interface IServiceEventBus : IDependency
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}
