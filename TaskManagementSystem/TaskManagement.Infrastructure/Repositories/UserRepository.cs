using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Where(u => u.Username == username)
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UserModel?> ValidateCredentialsAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            var user = await GetByUsernameAsync(username, cancellationToken);
            if (user == null) return null;

            var storedHash = await _context.Users
                .Where(u => u.Username == username)
                .Select(u => u.PasswordHash)
                .FirstOrDefaultAsync(cancellationToken);

            if (VerifyPasswordHash(password, storedHash))
            {
                return user;
            }

            return null;
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hashString == storedHash;
        }
    }
}