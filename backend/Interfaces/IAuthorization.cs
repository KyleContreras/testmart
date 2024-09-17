using backend.DTO;
namespace backend.Interfaces;

public interface IAuthorization
{
    public Task<(string? JwtToken, string? RefreshToken)> Login(LoginModel model);
    public Task Logout();
}