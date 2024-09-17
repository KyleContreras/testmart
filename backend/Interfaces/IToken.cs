using backend.Models;
namespace backend.Interfaces;

public interface IToken
{
    string GenerateJwtToken(ApplicationUser user);
    string GenerateRefreshToken();
}