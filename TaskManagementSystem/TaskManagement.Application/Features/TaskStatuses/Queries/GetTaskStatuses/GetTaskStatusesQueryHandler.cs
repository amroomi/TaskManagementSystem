using MediatR;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Features.TaskStatuses.Dto;

namespace TaskManagement.Application.Features.TaskStatuses.Queries.GetTaskStatuses
{
    public class GetTaskStatusesQueryHandler : IRequestHandler<GetTaskStatusesQuery, List<TaskStatusDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskStatusesQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<TaskStatusDto>> Handle(GetTaskStatusesQuery request, CancellationToken cancellationToken)
        {
            var statuses = await _taskRepository.GetAllStatusesAsync(cancellationToken);

            return statuses.Select(s => new TaskStatusDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToList();
        }
    }
}