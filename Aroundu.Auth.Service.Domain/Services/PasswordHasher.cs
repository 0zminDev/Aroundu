using Aroundu.SharedKernel.Interfaces.Dependencies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.Auth.Service.Domain.Services
{
    public interface IPasswordHasher : IService
    {
        string HashPassword(string password);
    }

    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
