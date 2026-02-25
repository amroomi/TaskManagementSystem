using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.TaskStatuses.Dto;
using TaskManagement.Application.Features.TaskStatuses.Queries.GetTaskStatuses;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskStatusesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllStatuses")]
        public async Task<ActionResult<List<TaskStatusDto>>> GetAllStatuses(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetTaskStatusesQuery(), cancellationToken);
            return Ok(result);
        }
    }
}