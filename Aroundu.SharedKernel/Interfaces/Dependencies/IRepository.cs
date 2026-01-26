namespace Aroundu.SharedKernel.Interfaces
{
    public interface IRepository : IDependency
    {
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
