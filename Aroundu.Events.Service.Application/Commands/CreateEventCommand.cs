using Aroundu.Events.Service.Application.IntegrationEvents;
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
        private readonly IServiceEventBus eventBus;

        public CreateEventCommandHandler(IEventRepository repo, IServiceEventBus eventBus)
        {
            this.repo = repo;
            this.eventBus = eventBus;
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken ct)
        {
            var entity = new Event { 
                Name = request.Name
            };

            await repo.AddAsync(entity);
            await eventBus.PublishAsync(new EventCreated(entity.Id, entity.Name), ct);
            await repo.SaveAsync(ct);


            return entity.Id;
        }
    }
}
