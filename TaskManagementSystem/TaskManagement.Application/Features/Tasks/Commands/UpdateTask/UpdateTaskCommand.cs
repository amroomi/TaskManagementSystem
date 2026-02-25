using MediatR;

namespace TaskManagement.Application.Features.Tasks.Commands.UpdateTask
{
    public record UpdateTaskCommand(int Id, string Title, int UserId, string? Description = null, int StatusId = 1) : IRequest<bool>
    {
        public int Id { get; init; } = Id;
        public string Title { get; init; } = Title;
        public int UserId { get; init; } = UserId;
        public string? Description { get; init; } = Description;
        public int StatusId { get; init; } = StatusId;
    }
}