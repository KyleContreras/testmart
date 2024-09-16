using backend.DTO;
using Microsoft.AspNetCore.Identity;
namespace backend.Interfaces;

public interface IAccount
{
    public Task<IdentityResult> RegisterAccount(RegisterModel model);
    public Task<IdentityResult> ConfirmEmail(string userId, string code);
    public Task<IdentityResult> DeleteAccount(string userId);
}