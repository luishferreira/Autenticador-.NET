namespace Autenticador.Domain.Entities
{
    public class Role
    {
        public int Id { get; init; }
        public required string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
