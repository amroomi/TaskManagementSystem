using MediatR;

namespace TaskManagement.Application.Features.Auth.Commands.ValidateLogin
{
    public record ValidateLoginCommand(string Username, string Password) : IRequest<LoginResult>;

    public record LoginResult(bool Success, string Token, int UserId, string Username);
}