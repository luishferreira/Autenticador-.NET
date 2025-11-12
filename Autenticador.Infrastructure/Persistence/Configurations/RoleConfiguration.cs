using Autenticador.Domain.Entities;
using Autenticador.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autenticador.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(r => r.Name)
                .IsUnique();

            builder.HasData(
                new Role { Id = (int)Roles.Admin, Name = nameof(Roles.Admin) },
                new Role { Id = (int)Roles.User, Name = nameof(Roles.User) }
            );
        }
    }
}
