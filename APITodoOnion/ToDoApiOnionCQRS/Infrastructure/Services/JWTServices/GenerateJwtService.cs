using Application.Interface;
using Domain.Dtos.User.JWT;
using Domain.Exceptions;
using Domain.Settings;
using Infrastructure.CustomEntities;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.JWTServices
{
    public class GenerateJwtService : IGenerateJwtService<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTSettings _jwtSettings;
        private readonly ILogger<GenerateJwtService> _logger;

        public GenerateJwtService(UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jwtSettings, ILogger<GenerateJwtService> logger)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<JWTResponse> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            if (userClaims.IsNullOrEmpty() || roles.IsNullOrEmpty())
            {
                _logger.LogInformation("User roles or claims doesn't exist");
                throw new ApiExceptions($"Los claims o roles del usuario {user.Id} no existen");
            }

            var rolesClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                rolesClaims.Add(new("roles", roles[i]));

            }
            string ipAddress = IpHelper.GetApiAddress();

            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("uid", user.Id),
                new("ip", ipAddress),
                new ("Active","True")
            }
            .Union(userClaims).Union(rolesClaims);

            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signInCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JWTResponse result = new();
            result.Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            result.userId = user.Id;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: result.Expires,
                signingCredentials: signInCredentials
                );

            result.Token = jwtSecurityToken;

            return result;
        }
    }
}
