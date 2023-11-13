using Domain.Dtos.User.Authentication;
using Domain.Dtos.User.JWT;
using LanguageExt.Common;

namespace Application.Interface
{
    public  interface IAccountService
    {
        public Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string apiAddress);
        public Task<Result<AuthenticationResponse>> RefreshJWTAsync(JWTRequest request, string ipAddress);
    }
}
