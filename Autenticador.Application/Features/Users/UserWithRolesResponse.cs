namespace Autenticador.Application.Features.Users
{
    public sealed record UserWithRolesResponse
    {
        public int Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public List<string> Roles { get; init; } = [];
        public UserWithRolesResponse() { }
    }
}
