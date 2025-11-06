using Autenticador.Domain.Entities;

namespace Autenticador.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<bool> UsernameExistsAsync(string username);
        Task<User?> GetByUsernameAsync(string username);
    }
}
