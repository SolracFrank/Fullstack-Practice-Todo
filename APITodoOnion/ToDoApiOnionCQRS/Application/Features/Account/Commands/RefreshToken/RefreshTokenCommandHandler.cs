using Application.Interface;
using Domain.Dtos.User.RefreshToken;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.Account.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenCommandHandler(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _refreshTokenService.RefreshTokenAsync(new RefreshTokenRequest
            {
                OldRefreshToken = request.OldRefreshToken,
                UserId = request.UserId,
            }, request.IpAddress);
        }
    }
}
