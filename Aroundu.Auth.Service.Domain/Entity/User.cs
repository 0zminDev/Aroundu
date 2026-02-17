#nullable enable

namespace Aroundu.Auth.Service.Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public Guid PublicKey { get; set; } = Guid.NewGuid();
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
    }
}
