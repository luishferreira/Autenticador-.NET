using Autenticador.Application.Common.Interfaces;
using Autenticador.Domain.Interfaces;

namespace Autenticador.Infrastructure.Services
{
    public class RedisPermissionCacheService(
        IRedisService redisService,
        IUserRepository userRepository) : IPermissionCacheService
    {
        private readonly IRedisService _redisService = redisService;
        private readonly IUserRepository _userRepository = userRepository;

        private static string GetCacheKey(int userId) => $"user_roles:{userId}";
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);

        public async Task<List<string>> GetRolesAsync(int userId)
        {
            var cacheKey = GetCacheKey(userId);

            var cachedRoles = await _redisService.GetAsync<List<string>>(cacheKey);

            if (cachedRoles != null)
                return cachedRoles;

            var user = await _userRepository.GetByIdWithRolesAsync(userId);

            if (user == null)
                return [];

            var roles = user.UserRoles
                .Select(ur => ur.Role.Name)
                .Distinct()
                .ToList();

            await _redisService.SetAsync(cacheKey, roles, _cacheDuration);

            return roles;
        }
        public async Task ClearRolesAsync(int userId)
        {
            await _redisService.RemoveAsync(GetCacheKey(userId));
        }
        public async Task SetRolesAsync(int userId, List<string> roles)
        {
            var cacheKey = GetCacheKey(userId);
            await _redisService.SetAsync(cacheKey, roles, _cacheDuration);
        }
    }
}
