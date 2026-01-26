using Aroundu.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Events.Service.Infrastructure.EFCore
{
    public abstract class BaseRepository : IRepository
    {
        protected readonly EventsDbContext context;

        protected BaseRepository(EventsDbContext context)
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
