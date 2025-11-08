using Autenticador.Application.Features.Auth.Login;
using Autenticador.Application.Features.Auth.Logout;
using Autenticador.Application.Features.Auth.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    }
}