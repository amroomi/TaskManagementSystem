using MediatR;
using TaskManagement.Application.Features.Tasks.Dto;

namespace TaskManagement.Application.Features.Tasks.Queries.GetTasks
{
    public record GetTasksQuery : IRequest<List<TaskDto>>;
}