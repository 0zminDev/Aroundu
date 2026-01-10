using Aroundu.Events.Service.Application.Repositories;
using Aroundu.Events.Service.Domain.Entity;
using Aroundu.Events.Service.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Infrastructure.Repositories
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        public EventRepository(EventsDbContext context) : base(context) {}

        public async Task AddAsync(Event eventEntity)
        {
            await Data<Event>().AddAsync(eventEntity);
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await Data<Event>().FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
