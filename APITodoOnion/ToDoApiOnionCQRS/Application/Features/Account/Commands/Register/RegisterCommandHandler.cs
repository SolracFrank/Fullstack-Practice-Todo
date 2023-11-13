using Application.Interface;
using Domain.Dtos.User;
using Domain.Dtos.User.Register;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.Account.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<string>>
    {
        private readonly IRegisterService _registerService;

        public RegisterCommandHandler(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _registerService.RegisterAsync(new RegisterRequest
            {
                Email = request.Email,
                UserName = request.UserName, 
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                LastName = request.LastName,
                Name = request.Name
            }, request.Origin);
        }
    }
}
