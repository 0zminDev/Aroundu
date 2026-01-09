using Aroundu.Events.Service.Domain.Entity;
using Aroundu.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Application.Repositories
{
    public interface IEventRepository : IRepository
    {
        Task AddAsync(Event eventEntity);

        Task<Event?> GetByIdAsync(int id);
    }
}
