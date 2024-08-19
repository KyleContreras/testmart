using backend.DTO;
using Microsoft.AspNetCore.Identity;

namespace backend.Interfaces;

public interface IAccount
{
    public Task<IdentityResult> RegisterAsync(RegisterModel model);
    public Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
    public Task<IdentityResult> DeleteAccountAsync(string userId);
}