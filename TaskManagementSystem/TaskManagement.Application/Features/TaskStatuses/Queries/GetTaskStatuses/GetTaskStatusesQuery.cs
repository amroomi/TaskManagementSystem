using MediatR;
using TaskManagement.Application.Features.TaskStatuses.Dto;

namespace TaskManagement.Application.Features.TaskStatuses.Queries.GetTaskStatuses
{
    public record GetTaskStatusesQuery : IRequest<List<TaskStatusDto>>;
}