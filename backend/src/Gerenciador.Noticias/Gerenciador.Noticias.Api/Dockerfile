# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia arquivos de projeto (para restaurar dependências)
COPY ["src/Gerenciador.Noticias/Gerenciador.Noticias.Api/Gerenciador.Noticias.Api.csproj", "Gerenciador.Noticias.Api/"]
COPY ["src/Gerenciador.Noticias/Gerenciador.Noticias.Application/Gerenciador.Noticias.Application.csproj", "Gerenciador.Noticias.Application/"]
COPY ["src/Gerenciador.Noticias/Gerenciador.Noticias.Domain/Gerenciador.Noticias.Domain.csproj", "Gerenciador.Noticias.Domain/"]
COPY ["src/Gerenciador.Noticias/Gerenciador.Noticias.Infra.Mongo/Gerenciador.Noticias.Infra.Mongo.csproj", "Gerenciador.Noticias.Infra.Mongo/"]
COPY ["src/Gerenciador.Noticias/Gerenciador.Noticias.Infra.Sql/Gerenciador.Noticias.Infra.Sql.csproj", "Gerenciador.Noticias.Infra.Sql/"]

RUN dotnet restore "Gerenciador.Noticias.Api/Gerenciador.Noticias.Api.csproj"

# Copia o restante do código mantendo estrutura correta
COPY src/ ./src/

# Compila
WORKDIR /src/src/Gerenciador.Noticias/Gerenciador.Noticias.Api
RUN dotnet build "Gerenciador.Noticias.Api.csproj" -c Release -o /app/build

# Publica
RUN dotnet publish "Gerenciador.Noticias.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final (runtime .NET 9)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Gerenciador.Noticias.Api.dll"]