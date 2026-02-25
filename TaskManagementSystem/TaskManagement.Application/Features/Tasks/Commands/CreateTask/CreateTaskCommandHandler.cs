using MediatR;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Features.Tasks.Commands.CreateTask
{
    internal class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required.", nameof(request.Title));

            var task = new TaskModel
            {
                Title = request.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
                StatusId = request.StatusId,
                IsDeleted = request.IsDeleted,
                UserId = request.UserId
            };

            return await _taskRepository.CreateAsync(task, cancellationToken);
        }
    }
}