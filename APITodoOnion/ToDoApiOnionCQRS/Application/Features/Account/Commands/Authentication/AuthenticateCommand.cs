using Domain.Dtos.User.Authentication;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.Account.Commands.Authentication
{
    public class AuthenticateCommand : IRequest<Result<AuthenticationResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public int UserId { get; set; }
    }

}
