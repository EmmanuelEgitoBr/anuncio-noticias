using Gerenciador.Noticias.Application.Mappings;
using Gerenciador.Noticias.Application.Services;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Interfaces;
using Gerenciador.Noticias.Infra.Mongo.Repositories;
using Gerenciador.Noticias.Infra.Mongo.Settings;
using Microsoft.Extensions.Options;
using System;

namespace Gerenciador.Noticias.Api.Extensions;

public static class WebApiBuilderExtensions
{
    public static void AddAutoMapperConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(MappingConfig));

    }

    public static void AddApplicationConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<INewsService, NewsService>();
        builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
    }

    public static void AddMongoConfig(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MongoDatabaseSettings>(
            builder.Configuration.GetSection("DatabaseSettings"));

        builder.Services.AddSingleton<IMongoDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
    }

    //public static void AddSqlConfiguration(this WebApplicationBuilder builder)
    //{
    //    builder.Services.AddDbContext<AppDbContext>(options =>
    //    {
    //        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
    //        options.EnableSensitiveDataLogging();
    //    });
    //}
}
