using Domain.Dtos.User.RefreshToken;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.Account.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<Result<RefreshTokenResponse>>
    {
        public string OldRefreshToken { get; set; }
        public string UserId { get; set; }
        public string IpAddress { get; set; }
    }
}
