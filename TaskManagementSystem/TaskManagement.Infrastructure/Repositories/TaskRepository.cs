using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Infrastructure.Persistence;
using DataHistory = TaskManagement.Infrastructure.Persistence.Models.TaskHistory;
using DataTask = TaskManagement.Infrastructure.Persistence.Models.Task;

namespace TaskManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Include(t => t.Status)
                .AsNoTracking()
                .Where(t => t.IsDeleted == false)
                .Select(t => new TaskModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsDeleted = t.IsDeleted,
                    StatusId = t.StatusId,
                    StatusName = t.Status.Name
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Include(t => t.Status)
                .Where(t => t.IsDeleted == false && t.Id == id)
                .Select(t => new TaskModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsDeleted = t.IsDeleted,
                    StatusId = t.StatusId,
                    StatusName = t.Status.Name
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TaskStatusModel>> GetAllStatusesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.TaskStatuses
                .AsNoTracking()
                .Select(s => new TaskStatusModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserModel>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CreateAsync(TaskModel task, CancellationToken cancellationToken = default)
        {
            var entity = new DataTask
            {
                Title = task.Title,
                Description = task.Description,
                StatusId = task.StatusId,
                IsDeleted = false
            };

            _context.Tasks.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            await AddHistoryAsync(entity.Id, task.UserId, "Created", cancellationToken);

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(TaskModel task, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == task.Id, cancellationToken);

            if (entity == null) return false;

            entity.Title = task.Title;
            entity.Description = task.Description;
            entity.StatusId = task.StatusId;

            await _context.SaveChangesAsync(cancellationToken);

            await AddHistoryAsync(task.Id, task.UserId, "Updated", cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (entity == null) return false;

            entity.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            await AddHistoryAsync(id, userId, "Deleted", cancellationToken);

            return true;
        }

        private async System.Threading.Tasks.Task AddHistoryAsync(int taskId, int userId, string action, CancellationToken cancellationToken)
        {
            var history = new DataHistory
            {
                TaskId = taskId,
                UserId = userId,
                Action = action,
            };

            _context.TaskHistories.Add(history);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}