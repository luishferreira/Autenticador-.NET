namespace Autenticador.Application.Common.Interfaces
{
    public interface IPermissionCacheService
    {
        /// <summary>
        /// Obtém as roles de um usuario, caso não ache, busca no SQL
        /// </summary>
        Task<List<string>> GetRolesAsync(int userId);

        /// <summary>
        /// Invalida (apaga) o cache de roles de um usuario.
        /// (Deve ser chamado quando um admin muda as permissões).
        /// </summary>
        Task ClearRolesAsync(int userId);

        /// <summary>
        /// Define o cache de roles para um usuário.
        /// </summary>
        Task SetRolesAsync(int userId, List<string> roles);
    }
}
