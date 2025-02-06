using Domain.Models.Http;
using Domain.Models;
using Domain.DTOs;

public interface IUserService
{
    Task<ApiResponse<string>> RegisterUserAsync(UserRegisterDto userDto);
    Task<ApiResponse<string>> AuthenticateUserAsync(UserLoginDto loginDto);
}
