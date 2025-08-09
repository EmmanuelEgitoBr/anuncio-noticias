using Gerenciador.Noticias.Api.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "wwwroot", // Define explicitamente
    Args = args              // Repassa os argumentos da linha de comando
});

// Add services to the container.

builder.Services.AddControllers();
builder.AddAutoMapperConfiguration();
builder.AddRedisConfiguration();
builder.AddApplicationConfig();
builder.AddMongoConfig();
builder.AddSqlConfiguration();
builder.AddSwaggerDoc();
builder.AddCorsConfig();
builder.AddHealthChecksConfiguration();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _=>true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.MapControllers();

app.Run();
