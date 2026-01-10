using Aroundu.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Infrastructure.EFCore
{
    public abstract class BaseRepository : IRepository
    {
        protected readonly EventsDbContext _context;

        protected BaseRepository(EventsDbContext context)
        {
            _context = context;
        }

        protected DbSet<T> Data<T>() where T : class
        {
            return _context.Set<T>();
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
