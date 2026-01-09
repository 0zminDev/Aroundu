using Aroundu.Events.Service.Application.Repositories;
using Aroundu.Events.Service.Domain.Entity;
using Aroundu.SharedKernel.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Events.Service.Application.Commands
{
    public class CreateEventCommand : ICommand<int>
    {
        public string Name { get; set; }
    }

    public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand, int>
    {
        private readonly IEventRepository repo;

        public CreateEventCommandHandler(IEventRepository repo)
        {
            this.repo = repo;
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken ct)
        {
            var entity = new Event { 
                Name = request.Name
            };

            await repo.AddAsync(entity);
            await repo.SaveAsync(ct);

            return entity.Id;
        }
    }
}
