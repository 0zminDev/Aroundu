using Aroundu.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Infrastructure.EFCore
{
    public abstract class BaseQuery : IQuery
    {
        protected readonly EventsDbContext _context;

        protected BaseQuery(EventsDbContext context)
        {
            _context = context;
        }

        protected IQueryable<T> QueryFor<T>() where T : class
        {
            return _context.Set<T>().AsNoTracking();
        }
    }
}
