using Gerenciador.Noticias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerenciador.Noticias.Infra.Sql.EfMappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.HasKey(c => c.Id);
    }
}
