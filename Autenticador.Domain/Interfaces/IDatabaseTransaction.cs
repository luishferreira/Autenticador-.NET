namespace Autenticador.Domain.Interfaces;

/// <summary>
/// Abstrai uma transação de base de dados para que o Domain
/// não dependa de tipos do Entity Framework (como IDbContextTransaction).
/// IAsyncDisposable permite que seja usada com 'await using'.
/// </summary>
public interface IDatabaseTransaction : IAsyncDisposable
{
    /// <summary>
    /// Confirma (Commits) as mudanças da transação na base de dados.
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reverte (Rollbacks) as mudanças da transação.
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}