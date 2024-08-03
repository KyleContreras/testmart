using backend.DTO;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public interface IAccountService
{
    public Task<IdentityResult> RegisterAsync(RegisterModel model);
    public Task<string?> LoginAsync(LoginModel model);
    public Task LogoutAsync();
    public Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
    public Task<IdentityResult> DeleteAccountAsync(string userId);
}