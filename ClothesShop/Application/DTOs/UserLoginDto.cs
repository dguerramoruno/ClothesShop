using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;
public class UserLoginDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
