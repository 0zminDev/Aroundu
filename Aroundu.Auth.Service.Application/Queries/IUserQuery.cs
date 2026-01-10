using Aroundu.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Auth.Service.Application.Queries
{
    public interface IUserQuery : IQuery
    {
        public Task<int> GetUsersCountAsync();
    }
}
