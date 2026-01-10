using Aroundu.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Auth.Service.Application.IntegrationEvents
{
    public class UserCreatedEvent : IIntegrationEvent
    {
        public Guid UserId { get; }
        public string Username { get; }
        public string Email { get; }
        public UserCreatedEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    {
    }
}
