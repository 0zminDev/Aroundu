using Aroundu.Auth.Service.Domain.Entity;
using Aroundu.SharedKernel.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Auth.Service.Application.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task AddAsync(User eventEntity);

        Task<User?> GetByIdAsync(int id);
    }
}
