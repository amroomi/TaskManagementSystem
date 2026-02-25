using MediatR;
using TaskManagement.Application.Common.Interfaces;

namespace TaskManagement.Application.Features.Auth.Commands.ValidateLogin
{
    public class ValidateLoginCommandHandler : IRequestHandler<ValidateLoginCommand, LoginResult>
    {
        private readonly IUserRepository _userRepository;

        public ValidateLoginCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResult> Handle(ValidateLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.ValidateCredentialsAsync(request.Username, request.Password, cancellationToken);

            if (user == null)
            {
                return new LoginResult(false, "", 0, "");
            }

            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{request.Username}:{request.Password}"));

            return new LoginResult(true, $"Basic {token}", user.Id, user.Username);
        }
    }
}