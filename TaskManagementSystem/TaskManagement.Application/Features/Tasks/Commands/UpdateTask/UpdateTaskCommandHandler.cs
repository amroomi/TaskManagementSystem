using MediatR;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new ValidationException("Invalid task ID.");

            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ValidationException("Title is required.");

            return await _taskRepository.UpdateAsync(new()
            {
                Id = request.Id,
                Title = request.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                StatusId = request.StatusId,
                UserId = request.UserId
            }, cancellationToken);
        }
    }
}