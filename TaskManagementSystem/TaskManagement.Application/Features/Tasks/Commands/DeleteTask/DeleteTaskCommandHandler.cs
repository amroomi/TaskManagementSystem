using MediatR;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                throw new ArgumentException("Invalid task ID.", nameof(request.Id));

            return await _taskRepository.DeleteAsync(request.Id, request.UserId, cancellationToken);
        }
    }
}