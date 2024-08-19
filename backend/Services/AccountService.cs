using Microsoft.AspNetCore.Identity;
using System.Text.Encodings.Web;
using backend.DTO;
using backend.Interfaces;
using backend.Models;

namespace backend.Services
{
    public class AccountService : IAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountService> _logger;
        private readonly Microsoft.AspNetCore.Identity.UI.Services.IEmailSender _emailSender;

        public AccountService(
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration, 
            ILogger<AccountService> logger, 
            Microsoft.AspNetCore.Identity.UI.Services.IEmailSender emailSender 
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model) {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdUser = await _userManager.CreateAsync(user, model.Password);

            if (createdUser.Succeeded)
            {
                _logger.LogInformation("New user created.");

                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = $"https://{_configuration["ApplicationUrl"]}/account/confirmemail?userId={user.Id}&code={emailConfirmationToken}";

                await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                    $"<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Click to confirm</a>.");
            }

            return createdUser;
        }

        public async Task<IdentityResult> DeleteAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("User account deleted.");
            }

            return result;
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
}