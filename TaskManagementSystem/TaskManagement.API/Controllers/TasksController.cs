using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Tasks.Commands.CreateTask;
using TaskManagement.Application.Features.Tasks.Commands.DeleteTask;
using TaskManagement.Application.Features.Tasks.Commands.UpdateTask;
using TaskManagement.Application.Features.Tasks.Dto;
using TaskManagement.Application.Features.Tasks.Queries.GetTasks;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllTasks")]
        public async Task<ActionResult<List<TaskDto>>> GetAllTasks(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTasksQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPost("SaveTask")]
        public async Task<ActionResult<int>> SaveTask([FromBody] CreateTaskCommand command, CancellationToken cancellationToken)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            var commandWithUser = command with { UserId = userId };

            var id = await _mediator.Send(commandWithUser, cancellationToken);
            return CreatedAtAction(nameof(GetAllTasks), new { id }, id);
        }

        [HttpPut("UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            var commandWithUser = command with { UserId = userId };

            var updated = await _mediator.Send(commandWithUser, cancellationToken);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("DeleteTask/{id:int}")]
        public async Task<IActionResult> DeleteTask(int id, CancellationToken cancellationToken)
        {
            var userId = (int)HttpContext.Items["UserId"]!;

            var deleted = await _mediator.Send(new DeleteTaskCommand(id, userId), cancellationToken);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}