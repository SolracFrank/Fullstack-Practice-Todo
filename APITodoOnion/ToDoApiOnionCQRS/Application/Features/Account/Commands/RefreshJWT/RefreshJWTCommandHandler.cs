using Application.Interface;
using Domain.Dtos.User.Authentication;
using Domain.Dtos.User.JWT;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.Account.Commands.RefreshJWT
{
    public class RefreshJWTCommandHandler : IRequestHandler<RefreshJWTCommand, Result<AuthenticationResponse>>
    {
        private readonly IAccountService _accountService;

        public RefreshJWTCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Result<AuthenticationResponse>> Handle(RefreshJWTCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.RefreshJWTAsync(new JWTRequest
            {
                RefreshToken = request.RefreshToken,
                userId = request.userId,
                OldJwtToken = request.Token.ToString(),
            }, request.IpAddress
                );
        }
    }
}
