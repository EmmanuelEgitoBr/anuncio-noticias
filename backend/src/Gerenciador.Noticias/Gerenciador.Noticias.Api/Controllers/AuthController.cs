using Gerenciador.Noticias.Api.Services.Auth.Interfaces;
using Gerenciador.Noticias.Application.Dtos.Auth;
using Gerenciador.Noticias.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gerenciador.Noticias.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var userDto = await _userService.GetUserDtoByUserNameAsync(registerDto.Username);

            if(userDto is null)
            {
                return NotFound();
            }

            userDto = await _userService.CreateUserAsync(userDto, registerDto.Password);

            return CreatedAtAction(nameof(Register), new { id = userDto.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var userDto = await _userService.GetUserDtoByUserNameAsync(loginDto.UserName);
            if (userDto == null) return Unauthorized("Invalid credentials");

            var result = await _userService.VerifyHashedPasswordAsync(loginDto);
            if (!result) return Unauthorized("Invalid credentials");

            var token = _jwtService.GenerateToken(userDto);
            return Ok(new { token });
        }
    }
}
