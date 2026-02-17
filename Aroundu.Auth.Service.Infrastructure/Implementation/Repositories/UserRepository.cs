using Aroundu.Auth.Service.Application.Repositories;
using Aroundu.Auth.Service.Domain.Entity;
using Aroundu.Auth.Service.Infrastructure.EFCore;
using Aroundu.Auth.Service.Infrastructure.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Auth.Service.Infrastructure.Implementation.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(AuthDbContext context) : base(context) { }

        public async Task AddAsync(User userEntity)
        {
            await Data<User>().AddAsync(userEntity);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await Data<User>().FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
