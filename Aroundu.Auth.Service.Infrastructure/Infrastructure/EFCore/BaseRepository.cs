using Aroundu.Auth.Service.Infrastructure.EFCore;
using Aroundu.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Auth.Service.Infrastructure.Infrastructure.EFCore
{
    public abstract class BaseRepository : IRepository
    {
        protected readonly AuthDbContext context;

        protected BaseRepository(AuthDbContext context)
        {
            this.context = context;
        }

        protected DbSet<T> Data<T>() where T : class
        {
            return context.Set<T>();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
