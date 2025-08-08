using Gerenciador.Noticias.Application.Dtos.Auth;
using System.Security.Claims;

namespace Gerenciador.Noticias.Api.Services.Auth.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserDto userDto);
    ClaimsPrincipal? ValidateToken(string token);
}
