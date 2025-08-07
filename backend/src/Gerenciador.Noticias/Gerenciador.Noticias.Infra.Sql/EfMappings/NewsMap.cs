using Gerenciador.Noticias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerenciador.Noticias.Infra.Sql.EfMappings;

public class NewsMap : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.Property(n => n.Id).ValueGeneratedOnAdd();
        builder.HasKey(n => n.Id);
    }
}
