using Aroundu.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Application.Queries
{
    public interface IEventQuery : IQuery
    {
        public Task<int> GetEventCountAsync();
    }
}
