using Gerenciador.Noticias.Api.Configurations;
using Gerenciador.Noticias.Api.Services.Auth;
using Gerenciador.Noticias.Api.Services.Auth.Interfaces;
using Gerenciador.Noticias.Application.Mappings;
using Gerenciador.Noticias.Application.Services;
using Gerenciador.Noticias.Application.Services.Cache;
using Gerenciador.Noticias.Application.Services.Cache.Interfaces;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Interfaces;
using Gerenciador.Noticias.Infra.Mongo.Repositories;
using Gerenciador.Noticias.Infra.Mongo.Settings;
using Gerenciador.Noticias.Infra.Sql.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Reflection;
using System.Text;
using MyMongoSettings = Gerenciador.Noticias.Infra.Mongo.Settings.AppMongoDatabaseSettings;


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
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IGalleryService, GalleryService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

        builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
        builder.Services.AddSingleton<ICacheService, RedisCacheService>();
    }

    public static void AddMongoConfig(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppMongoDatabaseSettings>(
            builder.Configuration.GetSection("AppMongoDatabaseSettings"));

        builder.Services.AddSingleton<IMongoDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<MyMongoSettings>>().Value);
    }

    public static void AddCorsConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular",
                policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200") // endereço do Angular
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
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

    public static void AddAuthConfiguration(this WebApplicationBuilder builder)
    {
        // Jwt settings bind
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

        // Add JwtService
        builder.Services.AddScoped<IJwtService, JwtService>();

        // Authentication setup
        var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });
    }

    public static void AddHealthChecksConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(builder.Configuration
                .GetSection("AppMongoDatabaseSettings:ConnectionString").Value!.ToString()));

        builder.Services.AddHealthChecks()
            .AddSqlServer(
                connectionString: builder.Configuration.GetConnectionString("SqlConnection")!,
                name: "sqlserver",
                tags: ["db", "sql"]
            )
            .AddMongoDb(
                clientFactory: sp => sp.GetRequiredService<IMongoClient>(),
                databaseNameFactory: sp => "api-news",
                name: "mongodb",
                tags: ["db", "mongo"]
            )
            .AddRedis(
                redisConnectionString: builder.Configuration.GetConnectionString("RedisConnection")!,
                name: "redis",
                tags: ["cache", "redis"]
    );

        builder.Services.AddHealthChecksUI(setup =>
        {
            builder.Configuration.GetSection("HealthChecksUI").Bind(setup);
        }).AddInMemoryStorage();
    }

    public static void AddRedisConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "MeuApp:"; // prefixo das chaves no Redis
        });
    }

}
