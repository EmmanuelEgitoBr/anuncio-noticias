using Gerenciador.Noticias.Application.Mappings;
using Gerenciador.Noticias.Application.Services;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Interfaces;
using Gerenciador.Noticias.Infra.Mongo.Repositories;
using Gerenciador.Noticias.Infra.Mongo.Settings;
using Gerenciador.Noticias.Infra.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;

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
            builder.Configuration.GetSection("MongoDatabaseSettings"));

        builder.Services.AddSingleton<IMongoDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
    }

    public static void AddSwaggerDoc(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "API de gerenciamento de notícias",
                Description = "API feita com .NET Core 9 usando MongoDB",
                Contact = new OpenApiContact
                {
                    Name = "Página de contato",
                    Url = new Uri("https://www.google.com")
                },
                License = new OpenApiLicense
                {
                    Name = "Licenciamento",
                    Url = new Uri("https://www.google.com")
                }
            });
            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
        });
    }

    public static void AddSqlConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
            options.EnableSensitiveDataLogging();
        });
    }
}
