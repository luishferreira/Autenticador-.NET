using Autenticador.Domain.Interfaces;

namespace Autenticador.Domain.Entities
{
    public abstract class BaseEntity : IAuditable
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
