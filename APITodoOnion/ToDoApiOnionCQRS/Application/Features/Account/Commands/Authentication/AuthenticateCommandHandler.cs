using Application.Interface;
using Domain.Dtos.User.Authentication;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.Account.Commands.Authentication
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, Result<AuthenticationResponse>>
    {
        private readonly IAccountService _accountService;
        public AuthenticateCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Result<AuthenticationResponse>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.AuthenticateAsync(new AuthenticationRequest
            {
                Email = request.Email,
                Password = request.Password,
            }, request.IpAddress);
        }
    }
}
