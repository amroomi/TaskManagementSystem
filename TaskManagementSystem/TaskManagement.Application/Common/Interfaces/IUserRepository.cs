using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<UserModel?> ValidateCredentialsAsync(string username, string password, CancellationToken cancellationToken = default);
    }
}