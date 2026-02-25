using MediatR;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Features.Tasks.Dto;

namespace TaskManagement.Application.Features.Tasks.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, List<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllAsync(cancellationToken);

            //throw new Exception("Exception");

            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsDeleted = t.IsDeleted,
                StatusId = t.StatusId,
                StatusName = t.StatusName,
            }).ToList();
        }
    }
}