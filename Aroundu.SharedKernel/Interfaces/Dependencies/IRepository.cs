using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.SharedKernel.Interfaces
{
    public interface IRepository : IDependency
    {
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
