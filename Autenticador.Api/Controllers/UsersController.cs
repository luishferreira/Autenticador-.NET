using Autenticador.Application.Features.Users;
using Autenticador.Application.Features.Users.Create;
using Autenticador.Application.Features.Users.CreateUser;
using Autenticador.Application.Features.Users.GetUserById;
using Autenticador.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Autenticador.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Obtém os detalhes de um utilizador específico pelo seu ID.
        /// </summary>
        /// <remarks>
        /// Algum remark
        /// </remarks>
        /// <param name="id" example="1">O ID do utilizador a ser procurado.</param>
        /// <param name="cancellationToken">Um token para cancelar a operação (injetado automaticamente).</param>
        /// <response code="200">Retorna os detalhes do utilizador.</response>
        /// <response code="400">ID de utilizador inválido (Validation Error).</response>
        /// <response code="404">Utilizador não encontrado (Not Found).</response>
        [HttpGet("{id:int}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(id);
            var response = await mediator.Send(query, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Cria um novo usuário no sistema
        /// </summary>
        /// <param name="command">Dados do Usuario(Nome, Password).</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <response code="201">Utilizador criado com sucesso. Retorna o ID do novo utilizador.</response>
        /// <response code="400">Dados de entrada inválidos (Validation Error).</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            var userId = await mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(Register), new { id = userId }, userId);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetDetails(CancellationToken cancellationToken)
        {
            int userId = GetUserIdFromClaims();

            var query = new GetUserByIdQuery(userId);
            var response = await mediator.Send(query, cancellationToken);

            return Ok(response);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserByAdmin(
            [FromBody] CreateUserAdminCommand command,
            CancellationToken cancellationToken)
        {
            var userId = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(CreateUserByAdmin), new { id = userId }, userId);
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
