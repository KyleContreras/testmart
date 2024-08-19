using backend.DTO;
namespace backend.Interfaces;

public interface IAuthorization
{
    public Task<string?> LoginAsync(LoginModel model);
    public Task LogoutAsync();
}