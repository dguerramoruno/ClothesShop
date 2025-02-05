using Domain.Models;

namespace Application.Service
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
