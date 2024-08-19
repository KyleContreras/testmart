using backend.Models;
namespace backend.Interfaces;

public interface IToken
{
    string GenerateJwtToken(ApplicationUser user);
    //string RefreshJwtToken(ApplicationUser user, string token);
    //string BlacklistJwtToken(ApplicationUser user, string token);
}