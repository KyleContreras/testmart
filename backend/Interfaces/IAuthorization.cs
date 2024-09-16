using backend.DTO;
namespace backend.Interfaces;

public interface IAuthorization
{
    public Task<string?> Login(LoginModel model);
    public Task Logout();
}