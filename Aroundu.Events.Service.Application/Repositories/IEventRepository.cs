using Aroundu.Events.Service.Domain.Entity;
using Aroundu.SharedKernel.Interfaces;

namespace Aroundu.Events.Service.Application.Repositories
{
    public interface IEventRepository : IRepository
    {
        Task AddAsync(Event eventEntity);

        Task<Event?> GetByIdAsync(int id);
    }
}
