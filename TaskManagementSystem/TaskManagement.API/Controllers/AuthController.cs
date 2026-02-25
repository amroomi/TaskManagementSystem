using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Auth.Commands.ValidateLogin;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Validate")]
        public async Task<ActionResult<LoginResult>> Validate([FromBody] ValidateLoginRequest request)
        {
            var command = new ValidateLoginCommand(request.Username, request.Password);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(new
            {
                success = true,
                token = result.Token,
                userId = result.UserId,
                username = result.Username
            });
        }
    }

    public class ValidateLoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}