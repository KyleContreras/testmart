using Microsoft.AspNetCore.Identity;
using backend.DTO;
using backend.Interfaces;
using backend.Models;
namespace backend.Services;

public class Account : IAccount
{
    private readonly UserManager<ApplicationUser> _userManager;

    public Account(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> RegisterAccount(RegisterModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdUser = await _userManager.CreateAsync(user, model.Password);

        return createdUser;
    }

    public async Task<IdentityResult> DeleteAccount(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        var result = await _userManager.DeleteAsync(user);

        return result;
    }
    
    public async Task<IdentityResult> ConfirmEmail(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = $"Unable to load user with ID '{userId}'." });
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        return result;
    }
}