using System.Diagnostics.CodeAnalysis;
using Aroundu.SharedKernel.Interfaces;

namespace Aroundu.SharedKernel.IntegrationEvents
{
    public class UserCreatedEvent : IIntegrationEvent
    {
        public required Guid UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }

        public UserCreatedEvent() { }
        
        [SetsRequiredMembers]
        public UserCreatedEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    }
}
