using Aroundu.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Events.Service.Infrastructure.EFCore
{
    public abstract class BaseQuery : IQuery
    {
        protected readonly EventsDbContext context;

        protected BaseQuery(EventsDbContext context)
        {
            this.context = context;
        }

        protected IQueryable<T> QueryFor<T>() where T : class
        {
            return context.Set<T>().AsNoTracking();
        }
    }
}
