using Domain.Dtos.User.JWT;

namespace Application.Interface
{
    public interface IGenerateJwtService <Tentity> where Tentity : class
    {
        public Task<JWTResponse> GenerateJwtTokenAsync(Tentity user);
    }
}
