using LanguageExt.Common;
using MediatR;

namespace Application.Features.Account.Commands.Register
{
    public class RegisterCommand : IRequest<Result<string>>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Origin { get; set; }
    }
}
