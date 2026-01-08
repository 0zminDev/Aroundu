using Aroundu.Events.Service.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Infrastructure.Queries
{
    public class EventQuery : IEventQuery
    {
        public async Task<int> GetEventCountAsync()
        {
            return await Task.FromResult(42); // Example static count
        }
    }
}
