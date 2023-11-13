using Application.Interface;
using Domain.Dtos.User.Authentication;
using Domain.Dtos.User.JWT;
using Domain.Dtos.User.RefreshToken;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.CustomEntities;
using Infrastructure.Helpers;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IGenerateJwtService<ApplicationUser> _generateJwtService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountService> logger, IUnitOfWork unitOfWork, IGenerateJwtService<ApplicationUser> generateJwtService, IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _generateJwtService = generateJwtService;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogInformation("User with the specified email doesn't exist");
                throw new ApiExceptions($"El usuario con el email {request.Email} no existe");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                _logger.LogInformation("Email or password aren't valid");
                throw new ApiExceptions($"El email o la contraseña no son válidos");
            }
            var todoUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.IdAccount == user.Id, CancellationToken.None);

            int idTodoUser = todoUser.IdUser;


            JWTResponse jwtResult = await _generateJwtService.GenerateJwtTokenAsync(user);
            JwtSecurityToken jwtSecurityToken = jwtResult.Token;

            AuthenticationResponse response = new();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;

            var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = roleList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWTExpires = jwtResult.Expires;
            response.UserId = idTodoUser;

            var refreshToken = GenerateRefreshToken(ipAddress, user);
            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken, CancellationToken.None);

            
            if (await _unitOfWork.SaveChangesAsync(CancellationToken.None))
            {
                response.RefreshToken = refreshToken.Token;
                response.RefreshTokenExpires = refreshToken.Expires;
            }
            else
            {
                _logger.LogInformation("Failed to save RefreshToken");
                throw new ApiExceptions("Ocurrio un error al guardar el toquen de refrescado");
            }

            return new Result<AuthenticationResponse>(response);
        }
        private RefreshToken GenerateRefreshToken(string ipAddress, ApplicationUser user)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = user.Id,
                Token = RefreshTokenHelper.RandomTokenString(),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                CreatedByIp = ipAddress,
            };
        }
        public async Task<Result<AuthenticationResponse>> RefreshJWTAsync (JWTRequest request,string ipAddress)
        {
            
            var user = await _userManager.FindByIdAsync(request.userId);
            if (user == null)
            {
                _logger.LogInformation("User with the specified email doesn't exist");
                throw new ApiExceptions($"El usuario con el email {request.userId} no existe");
            }

          
            var refreshTokenExists = await _unitOfWork.RefreshTokenRepository.AnyAsync(x => x.Token == request.RefreshToken, CancellationToken.None);

            if (!refreshTokenExists)
            {
                _logger.LogInformation("RefreshToken doesn't exist");
                throw new ApiExceptions($"El token de refrescado ${request.RefreshToken} no existe");
            }

           
            var oldRefreshToken = await _unitOfWork.RefreshTokenRepository.FirstOrDefaultAsync(x=>x.Token == request.RefreshToken, CancellationToken.None);
   
            if(oldRefreshToken.IsExpired)
            {
                _logger.LogInformation("RefreshToken has expired");
                throw new ApiExceptions($"El token de refrescado ${request.RefreshToken} ha expirado");
            }
            try
            {
            //var oldJwtSecurityToken = new JwtSecurityToken(request.OldJwtToken);
            //var oldUserIdClaim = oldJwtSecurityToken.Claims.First(c => c.Type == "uid").Value;

            //if (oldUserIdClaim != request.userId)
            //{
            //    _logger.LogInformation("Old JWT isn't a valid Token");
            //    throw new ApiExceptions("El JWT antiguo especificado no es válido");
            //}

            JWTResponse jwtResult = await _generateJwtService.GenerateJwtTokenAsync(user);
            JwtSecurityToken jwtSecurityToken = jwtResult.Token;

                var todoUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.IdAccount == user.Id, CancellationToken.None);

                

                AuthenticationResponse response = new();
            response.Id = user.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.UserId = todoUser.IdUser;

                var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = roleList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWTExpires = jwtResult.Expires;

            RefreshTokenRequest refreshRequest = new();
            refreshRequest.UserId = user.Id;
            refreshRequest.OldRefreshToken = oldRefreshToken.Token;

            var refreshToken = await _refreshTokenService.RefreshTokenAsync(refreshRequest,ipAddress);

            if(refreshToken.IsFaulted)
            {
                _logger.LogInformation("Failed to generate new RefreshToken");
                throw new ApiExceptions("Ocurrio un error al guardar el toquen de refrescado");
            }

            var newRefreshToken = await _unitOfWork.RefreshTokenRepository.FirstOrDefaultAsync(x => x.ReplacedByToken == request.RefreshToken, CancellationToken.None);


            if (newRefreshToken != null)
            {
                response.RefreshToken = newRefreshToken.Token;
                response.RefreshTokenExpires = newRefreshToken.Expires;
            }
            else
            {
                _logger.LogInformation("Failed to save RefreshToken");
                throw new ApiExceptions("Ocurrio un error al guardar el toquen de refrescado");
            }

            return new Result<AuthenticationResponse>(response);
            }
            catch (ApiExceptions ex)
            {
                throw new ApiExceptions($"Ocurrio un error: {ex.Message}");
            }
        }

    }
}
