using Aroundu.Events.Service.Application.Queries;
using Aroundu.Events.Service.Domain.Entity;
using Aroundu.Events.Service.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Events.Service.Infrastructure.Queries
{
    public class EventQuery : BaseQuery, IEventQuery
    {
        public EventQuery(EventsDbContext context) : base(context) {}

        public async Task<int> GetEventsCountAsync()
        {
            return await QueryFor<Event>().CountAsync();
        }
    }
}
