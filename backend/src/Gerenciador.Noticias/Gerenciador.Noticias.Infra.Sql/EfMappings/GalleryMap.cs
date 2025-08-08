using Gerenciador.Noticias.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerenciador.Noticias.Infra.Sql.EfMappings;

public class GalleryMap : IEntityTypeConfiguration<Gallery>
{
    public void Configure(EntityTypeBuilder<Gallery> builder)
    {
        builder.Property(g => g.Id).ValueGeneratedOnAdd();
        builder.HasKey(g => g.Id);
    }
}
