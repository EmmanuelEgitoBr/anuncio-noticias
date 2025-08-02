using Gerenciador.Noticias.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador.Noticias.Infra.Sql.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<News> News { get; set; }
}
