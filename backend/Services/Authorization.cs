using backend.DTO;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public class Authorization : IAuthorization
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IToken _tokenService;

    public Authorization(
        SignInManager<ApplicationUser> signInManager, 
        UserManager<ApplicationUser> userManager, 
        IToken tokenService
        )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
    }
    
    
    public async Task<(string? JwtToken, string? RefreshToken)> Login(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
            isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var jwt = _tokenService.GenerateJwtToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            return (jwt, refreshToken);
        }

        return (null, null);
    }

    
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }
}