using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public class EmailSenderService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Microsoft.AspNetCore.Identity.UI.Services.IEmailSender _emailSender;
    
    public EmailSenderService(UserManager<ApplicationUser> userManager, Microsoft.AspNetCore.Identity.UI.Services.IEmailSender emailSender) {

    }
    
    
    public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
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