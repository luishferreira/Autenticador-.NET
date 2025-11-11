using Autenticador.Domain.Entities;
using Autenticador.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace Autenticador.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public async Task<IDatabaseTransaction> BeginTransactionAsync(CancellationToken ct = default)
    {
        var efTransaction = await Database.BeginTransactionAsync(ct);
        return new DatabaseTransactionWrapper(efTransaction);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditable &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var auditable = (IAuditable)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                auditable.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                auditable.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

}

internal class DatabaseTransactionWrapper(IDbContextTransaction efTransaction) : IDatabaseTransaction
{
    public Task CommitAsync(CancellationToken ct) => efTransaction.CommitAsync(ct);
    public Task RollbackAsync(CancellationToken ct) => efTransaction.RollbackAsync(ct);
    public ValueTask DisposeAsync() => efTransaction.DisposeAsync();
}
