using Microsoft.AspNetCore.Mvc;
using Domain.Models.Http;
using Application.DTOs;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto)
    {
        var response = await _userService.RegisterUserAsync(userDto);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
    {
        var result = await _userService.AuthenticateUserAsync(loginDto);
        return StatusCode(result.StatusCode, result);
    }
}
