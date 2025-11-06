namespace Autenticador.Domain.Interfaces;

/// <summary>
/// Define o padrão Unit of Work.
/// Expõe o método para salvar mudanças e gerir transações.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Salva todas as mudanças feitas no contexto (DbContext) para a base de dados.
    /// Isto é o "Commit" principal das suas entidades (ex: AddAsync, UpdateAsync).
    /// </summary>
    /// <returns>O número de linhas afetadas.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Inicia uma nova transação de base de dados.
    /// Usado pelo UnitOfWorkBehaviour para garantir operações atómicas.
    /// </summary>
    /// <returns>Uma instância da transação (que pode ser "comitada" ou "revertida").</returns>
    Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}