using Gerenciador.Noticias.Domain.Entities;
using Gerenciador.Noticias.Domain.ValueObjects;
using Gerenciador.Noticias.Infra.Sql.EfMappings;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador.Noticias.Infra.Sql.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<News> News { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Gallery> Galleries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Media>().HasNoKey();

        modelBuilder.ApplyConfiguration(new NewsMap());
        modelBuilder.ApplyConfiguration(new CategoryMap());
        modelBuilder.ApplyConfiguration(new GalleryMap());

        base.OnModelCreating(modelBuilder);
    }
}
