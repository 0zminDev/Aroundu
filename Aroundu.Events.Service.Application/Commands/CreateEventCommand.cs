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
        private readonly IEventRepository eventRepository;
        private readonly IServiceEventBus eventBus;

        public CreateEventCommandHandler(IEventRepository repo, IServiceEventBus eventBus)
        {
            this.eventRepository = repo;
            this.eventBus = eventBus;
        }

        public async Task<int> Handle(CreateEventCommand request, CancellationToken ct)
        {
            var entity = new Event 
            { 
                Name = request.Name
            };

            await eventRepository.AddAsync(entity);
            await eventBus.PublishAsync(new EventCreatedEvent(entity.Id, entity.Name), ct);
            await eventRepository.SaveAsync(ct);


            return entity.Id;
        }
    }
}
