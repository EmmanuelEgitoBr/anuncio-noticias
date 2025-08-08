using Gerenciador.Noticias.Application.Dtos.Auth;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Gerenciador.Noticias.Domain.Entities.Auth;
using Gerenciador.Noticias.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Gerenciador.Noticias.Application.Services;

public class UserService : IUserService
{
    private readonly IMongoRepository<User> _repository;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IMongoRepository<User> repository)
    {
        _repository = repository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<UserDto> GetUserDtoByUserNameAsync(string userName)
    {
        var userEntity = await _repository.GetByPropertyAsync(u => u.UserName == userName);
        UserDto userDto = new()
        {
            UserName = userName,
            Email = userEntity!.Email,
            CPF = userEntity.CPF
        };
        return userDto;
    }

    public async Task<UserDto> GetUserDtoByCpfAsync(string cpf)
    {
        var userEntity = await _repository.GetByPropertyAsync(u => u.CPF == cpf);
        UserDto userDto = new()
        {
            UserName = userEntity!.UserName,
            Email = userEntity!.Email,
            CPF = userEntity.CPF
        };
        return userDto;
    }

    public async Task<UserDto> GetUserDtoByEmailAsync(string email)
    {
        var userEntity = await _repository.GetByPropertyAsync(u => u.Email == email);
        UserDto userDto = new()
        {
            UserName = userEntity!.UserName,
            Email = userEntity!.Email,
            CPF = userEntity.CPF
        };
        return userDto;
    }

    public async Task<UserDto> CreateUserAsync(UserDto userDto, string password)
    {
        var user = new User
        {
            UserName = userDto.UserName,
            Email = userDto.Email,
            Role = "User" // default role
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        user = await _repository.CreateAsync(user);
        userDto.Id = user.Id;
        userDto.Role = user.Role!;

        return userDto;
    }

    public async Task<bool> VerifyHashedPasswordAsync(LoginDto loginDto)
    {
        var user = await _repository.GetByPropertyAsync(u => u.UserName == loginDto.UserName);

        var result = _passwordHasher.VerifyHashedPassword(user!, user!.PasswordHash, loginDto.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return false;
        }
        return true;
    }
}
