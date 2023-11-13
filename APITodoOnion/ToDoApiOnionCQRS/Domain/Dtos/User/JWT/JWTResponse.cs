using System.IdentityModel.Tokens.Jwt;

namespace Domain.Dtos.User.JWT
{
    public class JWTResponse
    {
        public string userId { get; set; }
        public JwtSecurityToken Token { get; set; }
        public DateTime Expires { get; set; }
    }

}
