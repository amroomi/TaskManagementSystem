using MediatR;

namespace TaskManagement.Application.Features.Tasks.Commands.CreateTask
{
    public record CreateTaskCommand(string Title, int UserId, string? Description = null, int StatusId = 1, bool IsDeleted = false) : IRequest<int>
    {
        public string Title { get; init; } = Title;
        public int UserId { get; init; } = UserId;
        public string? Description { get; init; } = Description;
        public int StatusId { get; init; } = StatusId;
        public bool IsDeleted { get; init; } = IsDeleted;
    }
}