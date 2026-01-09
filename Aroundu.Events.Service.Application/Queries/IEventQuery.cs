using Aroundu.SharedKernel.Interfaces;

namespace Aroundu.Events.Service.Application.Queries
{
    public interface IEventQuery : IQuery
    {
        public Task<int> GetEventCountAsync();
    }
}
