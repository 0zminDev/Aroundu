using Aroundu.Auth.Service.Domain.Entity;
using Aroundu.SharedKernel.Interfaces;

namespace Aroundu.Auth.Service.Application.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task AddAsync(User eventEntity);

        Task<User?> GetByIdAsync(int id);
    }
}
