using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Common.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TaskModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TaskStatusModel>> GetAllStatusesAsync(CancellationToken cancellationToken = default);
        Task<List<UserModel>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<int> CreateAsync(TaskModel task, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(TaskModel task, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default);
    }
}