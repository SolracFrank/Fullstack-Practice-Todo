using Application.enums;
using Application.Interface;
using Domain.Dtos.User.Register;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.CustomEntities;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services.AccountServices
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterService> _logger;

        public RegisterService(UserManager<ApplicationUser> userManager, ILogger<RegisterService> logger, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            // Retry Transaction
            return await _unitOfWork.ExecuteWithinTransactionAsync(async () =>
            {
                var userWithUserName = await _userManager.FindByNameAsync(request.UserName);
                if (userWithUserName != null)
                {
                    _logger.LogInformation("User with specified username already exists");
                    throw new ApiExceptions($"El usuario {request.UserName} ya existe");
                }

                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    Name = request.Name,
                    LastName = request.LastName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };

                var userWithEmail = await _userManager.FindByEmailAsync(request.Email);
                if (userWithEmail != null)
                {
                    _logger.LogInformation("User with specified email already exists");
                    throw new ApiExceptions($"El email {request.Email} ya ha sido registrado");
                }

                // Inicia la transacción
                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    var result = await _userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                        var userClaims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim("uid", user.Id),
                            new Claim("ip", origin),
                            new Claim("Active","True")
                        };
                        await _userManager.AddClaimsAsync(user, userClaims);
                        _logger.LogInformation("Register successful");

                        var createTodoUserResult = await CreateTodoUserAsync(user.Id);
                        if (!createTodoUserResult)
                        {
                            throw new ApiExceptions("Error al crear el Todo User");
                        }

                        // Confirma la transacción
                        await _unitOfWork.CommitTransactionAsync(CancellationToken.None);
                        return new Result<string>($"El usuario {user.UserName} ha sido registrado satisfactoriamente");
                    }
                    else
                    {
                        _logger.LogInformation("Register failed");
                        throw new ApiExceptions($"{result.Errors}.");
                    }
                }
                catch (ApiExceptions ex)
                {
                    // Revierte la transacción
                    _unitOfWork.RollbackTransaction();
                    _logger.LogError(ex, "Un error ocurrió durante el registro");
                    throw new ApiExceptions($"Ocurrió un error: {ex.Message}");
                }
            });
        }
        private async Task<bool> CreateTodoUserAsync(string applicationUserId)
        {
            try
            {
                var user = new User
                {
                    IdAccount = applicationUserId,
                };
                await _unitOfWork.UserRepository.AddAsync(user, CancellationToken.None);

                return await _unitOfWork.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception)
            {
                _logger.LogInformation($"Error al crear el Todo User (identity id: {applicationUserId}");
                return false;
            }
        }
    }
}
