using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public interface IEmailSender
{
    public Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
}