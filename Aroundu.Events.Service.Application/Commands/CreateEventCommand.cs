using Aroundu.Events.Service.Application.Repositories;
using Aroundu.Events.Service.Domain.Entity;
using Aroundu.SharedKernel.IntegrationEvents;
using Aroundu.SharedKernel.Interfaces;

namespace Aroundu.Events.Service.Application.Commands
{
    public class CreateEventCommand : ICommand<Guid>
    {
        public string Name { get; set; }
    }

    public class CreateEventCommandHandler : ICommandHandler<CreateEventCommand, Guid>
    {
        private readonly IEventRepository eventRepository;
        private readonly IServiceEventBus eventBus;

        public CreateEventCommandHandler(IEventRepository repo, IServiceEventBus eventBus)
        {
            this.eventRepository = repo;
            this.eventBus = eventBus;
        }

        public async Task<Guid> Handle(CreateEventCommand request, CancellationToken ct)
        {
            var entity = new Event 
            {
                PublicKey = Guid.NewGuid(),
                Name = request.Name
            };

            await eventRepository.AddAsync(entity);
            await eventBus.PublishAsync(new EventCreatedEvent(entity.Id, entity.Name), ct);
            await eventRepository.SaveAsync(ct);


            return entity.PublicKey;
        }
    }
}
