using Application.Features.Account.Commands.Authentication;
using Application.Features.Account.Commands.RefreshJWT;
using Application.Features.Account.Commands.Register;
using Domain.Dtos.User.Authentication;
using Domain.Dtos.User.JWT;
using Domain.Dtos.User.Register;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : BaseApiController
    {
        [HttpPost("register")]
        [SwaggerResponse(StatusCodes.Status200OK, "Register succesful")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid inputs", typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
           var result = await Mediator.Send(new RegisterCommand
            {
                Name = request.Name,
                LastName = request.LastName,
                ConfirmPassword = request.ConfirmPassword,
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                Origin = Request.Headers["origin"],
            });

            return result.ToOk();
        }

        [HttpPost("login")]
        [SwaggerResponse(StatusCodes.Status200OK, "User found")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid user", typeof(ValidationProblemDetails))]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            var result  = await Mediator.Send(new AuthenticateCommand
            {
                Email = request.Email,
                Password = request.Password,
                IpAddress = GenerateIpAddress()
            });
            return result.ToOk();
        }

        [HttpPost("refreshJwt")]
        [SwaggerResponse(StatusCodes.Status200OK, "JWT Refreshed")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid token", typeof(ValidationProblemDetails))]
        public async Task<IActionResult> RefreshJWTAsync([FromBody] JWTRequest request)
        {
            var result = await Mediator.Send(new RefreshJWTCommand
            {
              Token = request.OldJwtToken,
              IpAddress = GenerateIpAddress(),
              userId = request.userId,
              RefreshToken = request.RefreshToken,
              
            });
            return result.ToOk();
        }

        private string GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
