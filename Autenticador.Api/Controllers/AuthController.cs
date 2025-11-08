using Autenticador.Application.Features.Auth.Login;
using Autenticador.Application.Features.Auth.Logout;
using Autenticador.Application.Features.Auth.LogoutAll;
using Autenticador.Application.Features.Auth.Refresh;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Autenticador.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(command, cancellationToken);
            SetTokenCookie(response.RefreshToken);

            return Ok(new { accessToken = response.AccessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                var command = new RefreshCommand(refreshToken);
                var response = await mediator.Send(command, cancellationToken);

                SetTokenCookie(response.RefreshToken);

                return Ok(new { accessToken = response.AccessToken });
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                var command = new LogoutCommand(refreshToken);
                await mediator.Send(command, cancellationToken);

                Response.Cookies.Delete("refreshToken");

                return NoContent();
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutFromAllDevices(CancellationToken cancellationToken)
        {
            var userId = GetUserIdFromClaims();

            var command = new LogoutAllCommand(userId);
            await mediator.Send(command, cancellationToken);

            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        /// <summary>
        /// Helper privado para ler o ID do utilizador a partir das
        /// Claims do Access Token.
        /// </summary>
        private int GetUserIdFromClaims()
        {
            var userIdClaim = (User.FindFirst(JwtRegisteredClaimNames.Sub)
                ?? User.FindFirst(ClaimTypes.NameIdentifier))
                ?? throw new UnauthorizedAccessException("User id not found in Access Token.");

            if (int.TryParse(userIdClaim.Value, out var userId))
                return userId;

            throw new UnauthorizedAccessException("User id not found in Access Token.");
        }
    }
}