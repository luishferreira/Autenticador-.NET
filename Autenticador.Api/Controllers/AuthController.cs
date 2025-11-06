using Autenticador.Application.Features.Auth.Login;
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

            var jsonResponse = new
            {
                success = true,
                accessToken = response.AccessToken
            };

            return Ok(jsonResponse);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                var command = new RefreshCommand(refreshToken);
                var response = await mediator.Send(command, cancellationToken);

                SetTokenCookie(response.RefreshToken);

                var jsonResponse = new
                {
                    success = true,
                    accessToken = response.AccessToken
                };

                return Ok(jsonResponse);
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            if (Request.Cookies.TryGetValue("refreshToken", out _))
            {
                Response.Cookies.Delete("refreshToken");

                return Ok(new { success = true, message = "Deslogado com sucesso." });
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