using Autenticador.Domain.Entities;
using Autenticador.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autenticador.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<User?> GetByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            await context.Users.AddAsync(user);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
