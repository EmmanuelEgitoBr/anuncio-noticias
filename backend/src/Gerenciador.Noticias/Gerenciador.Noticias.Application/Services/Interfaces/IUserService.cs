using Gerenciador.Noticias.Application.Dtos.Auth;

namespace Gerenciador.Noticias.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserDtoByUserNameAsync(string userName);
        Task<UserDto> GetUserDtoByCpfAsync(string userName);
        Task<UserDto> GetUserDtoByEmailAsync(string userName);
        Task<UserDto> CreateUserAsync(UserDto userDto, string password);
        Task<bool> VerifyHashedPasswordAsync(LoginDto loginDto);
    }
}
