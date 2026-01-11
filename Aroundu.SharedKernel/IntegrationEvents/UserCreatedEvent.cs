using Aroundu.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.SharedKernel.IntegrationEvents
{
    public class UserCreatedEvent : IIntegrationEvent
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public UserCreatedEvent() { }
        public UserCreatedEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    }
}
