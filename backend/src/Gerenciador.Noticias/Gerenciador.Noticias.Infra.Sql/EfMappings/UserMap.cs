using Gerenciador.Noticias.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerenciador.Noticias.Infra.Sql.EfMappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Nome da tabela
            builder.ToTable("Users");

            // Chave primária
            builder.HasKey(u => u.Id);

            // Propriedades
            builder.Property(u => u.Id)
                   .HasDefaultValueSql("NEWID()"); // gera GUID no banco se não informado

            builder.Property(u => u.UserName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.CPF)
                .IsRequired()
                .HasMaxLength(11)
                .IsFixedLength<string>(true);

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.Property(u => u.Role)
                   .HasMaxLength(50);
        }
    }
}
