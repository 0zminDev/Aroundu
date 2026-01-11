using Aroundu.SharedKernel.Interfaces;

namespace Aroundu.Auth.Service.Application.Queries
{
    public interface IUserQuery : IQuery
    {
        public Task<int> GetUsersCountAsync();
    }
}
