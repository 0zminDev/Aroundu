using Aroundu.Auth.Service.Infrastructure.EFCore;
using Aroundu.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Auth.Service.Infrastructure.Infrastructure.EFCore
{
    public abstract class BaseQuery : IQuery
    {
        protected readonly AuthDbContext context;

        protected BaseQuery(AuthDbContext context)
        {
            this.context = context;
        }

        protected IQueryable<T> QueryFor<T>() where T : class
        {
            return context.Set<T>().AsNoTracking();
        }
    }
}
