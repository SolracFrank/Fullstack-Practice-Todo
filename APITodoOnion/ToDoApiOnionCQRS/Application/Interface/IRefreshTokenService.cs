using Domain.Dtos.User.RefreshToken;
using LanguageExt.Common;

namespace Application.Interface
{
    public interface IRefreshTokenService
    {
        public Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
    }
}
