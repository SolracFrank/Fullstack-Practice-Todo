using Application.Interface;
using Domain.Dtos.User.RefreshToken;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Helpers;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.RefreshTokenServices
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<RefreshTokenService> _logger;

        public RefreshTokenService(IUnitOfWork unitOfWork, ILogger<RefreshTokenService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
        {
            if (string.IsNullOrEmpty(request?.OldRefreshToken) || string.IsNullOrEmpty(ipAddress))
            {
                _logger.LogInformation("Refresh token or IP address is null or empty");
                throw new ApiExceptions("Invalid request");
            }

            var isStoredToken = await _unitOfWork.RefreshTokenRepository.AnyAsync(x => x.Token == request.OldRefreshToken, CancellationToken.None);

            if (!isStoredToken)
            {
                _logger.LogInformation("Refresh Token doesn't exist");
                throw new ApiExceptions("El RefreshToken no existe");
            }

            var storedToken = await _unitOfWork.RefreshTokenRepository
                .FirstOrDefaultAsync(x => x.Token == request.OldRefreshToken && x.CreatedByIp == ipAddress, CancellationToken.None);
            if (storedToken == null)
            {
                _logger.LogInformation("Refresh Token is invalid");
                throw new ApiExceptions("El token de refresco es inválido.");
            }

            if (storedToken.IsExpired)
            {
                _logger.LogInformation("Refresh Token has expired");
                throw new ApiExceptions("El token de refresco ha expirado.");
            }

            if (storedToken.Revoked != null)
            {
                _logger.LogInformation("Refresh Token has been revoked");
                throw new ApiExceptions("El token de refresco ha sido revocado.");
            }
            storedToken.Revoked = DateTime.UtcNow;
            storedToken.RevokedByIp = ipAddress;
             _unitOfWork.RefreshTokenRepository.Update(storedToken);

            var newRefreshToken = UpdateRefreshToken(ipAddress, request.UserId, request.OldRefreshToken);

            await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken, CancellationToken.None);

            RefreshTokenResponse response = new();
            response.Expires = newRefreshToken.Expires;
            response.Token = newRefreshToken.Token;
            if (await _unitOfWork.SaveChangesAsync(CancellationToken.None))
            {
                return new Result<RefreshTokenResponse>(response);
            }
            else
            {
                _logger.LogInformation("Failed to save RefreshToken");
                throw new ApiExceptions("Ocurrio un error al guardar el toquen");
            }
        }

        private RefreshToken UpdateRefreshToken(string ipAddress, string userId, string oldToken)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = userId,
                Token = RefreshTokenHelper.RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                ReplacedByToken = oldToken,
                CreatedByIp = ipAddress,
            };
        }
    }
}
