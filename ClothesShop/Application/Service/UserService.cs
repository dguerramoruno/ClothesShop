using Domain.Models.Http;
using BCrypt.Net;
using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;
using Application.Service;
public class UserService : IUserService
{
    private readonly ProductDbContext _context;
    private readonly ITokenService _tokenService;
    public UserService(ProductDbContext context,ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<string>> RegisterUserAsync(UserRegisterDto userDto)
    {
        var userDb =  _context.Users.Where(u => u.Username == userDto.Username).Count();
        if (userDb != 0)
        {
            return new ApiResponse<string>(null, false, "El usuario ya existe.", 400);
        }

        var user = new User
        {
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return new ApiResponse<string>("Usuario registrado exitosamente.", true, null, 201);
    }
    public async Task<ApiResponse<string>> AuthenticateUserAsync(UserLoginDto loginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return new ApiResponse<string>(null, false, "Credenciales inválidas", 401);
        }

        var token = _tokenService.GenerateToken(user);
        return new ApiResponse<string>(token, true, null, 200);
    }

}
