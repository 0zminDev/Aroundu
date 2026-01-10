using Aroundu.Auth.Service.Application.Queries;
using Aroundu.Auth.Service.Domain.Entity;
using Aroundu.Auth.Service.Infrastructure.EFCore;
using Aroundu.Auth.Service.Infrastructure.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Aroundu.Auth.Service.Infrastructure.Implementation.Queries
{
    public class UserQuery : BaseQuery, IUserQuery
    {
        public UserQuery(AuthDbContext context) : base(context) {}

        public async Task<int> GetUsersCountAsync()
        {
            return await QueryFor<User>().CountAsync();
        }
    }
}
