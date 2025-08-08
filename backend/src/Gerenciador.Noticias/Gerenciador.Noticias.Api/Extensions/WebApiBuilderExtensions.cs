using Gerenciador.Noticias.Api.Configurations;
using Gerenciador.Noticias.Api.Services.Auth.Interfaces;
using Gerenciador.Noticias.Api.Services.Auth;
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
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
    }

    public static void AddMongoConfig(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MongoDatabaseSettings>(
            builder.Configuration.GetSection("MongoDatabaseSettings"));

        builder.Services.AddSingleton<IMongoDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);
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
}
