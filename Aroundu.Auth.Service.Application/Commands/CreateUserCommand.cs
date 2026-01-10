using Aroundu.Auth.Service.Application.IntegrationEvents;
using Aroundu.Auth.Service.Domain.Entity;
using Aroundu.Auth.Service.Domain.Services;
using Aroundu.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Auth.Service.Application.Commands
{
    public class CreateUserCommand : ICommand<Guid>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly IServiceEventBus eventBus;
        private readonly IPasswordHasher passwordHasher;

        public CreateUserCommandHandler(IUserRepository userRepository, IServiceEventBus eventBus, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.eventBus = eventBus;
            this.passwordHasher = passwordHasher;
        }
        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var entity = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = this.passwordHasher.HashPassword(request.Password)
            };

            await userRepository.AddAsync(entity);
            await eventBus.PublishAsync(new UserCreatedEvent(entity.PublicKey, entity.Username, entity.Email), ct);
            await userRepository.SaveAsync(ct);

            return entity.PublicKey;
        }
    }
}
