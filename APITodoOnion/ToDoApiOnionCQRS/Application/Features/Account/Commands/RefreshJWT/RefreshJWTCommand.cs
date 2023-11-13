using Domain.Dtos.User.Authentication;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.Account.Commands.RefreshJWT
{
    public class RefreshJWTCommand : IRequest<Result<AuthenticationResponse>>
    {
        public string IpAddress { get; set; }
        public string Token { get; set; }
        public string userId { get; set; }
        public string RefreshToken { get; set; }

    }
}
