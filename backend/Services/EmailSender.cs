using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text;

namespace backend.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;
        private static readonly string[] Scopes = { GmailService.Scope.GmailSend };

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var provider = _configuration["EmailProvider:Provider"];

            if (provider == "Gmail")
            {
                await SendEmailViaGmailApiAsync(email, subject, htmlMessage);
            }
            else
            {
                throw new NotImplementedException("Only Gmail API is currently supported.");
            }
        }

        private async Task SendEmailViaGmailApiAsync(string email, string subject, string htmlMessage)
        {
            var userEmail = _configuration["GmailApiSettings:UserEmail"];
            var credentialsPath = Path.Combine(AppContext.BaseDirectory, _configuration["GmailApiSettings:CredentialsPath"]);
            var tokenPath = Path.Combine(AppContext.BaseDirectory, _configuration["Google:TokenPath"]);
            var clientId = _configuration["Google:ClientId"];
            var clientSecret = _configuration["Google:ClientSecret"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("ClientId and ClientSecret must be set in the user secrets.");
            }

            UserCredential credential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                var clientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                };

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenPath, true));
            }

            var service = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "YourAppName",
            });

            var plainTextBytes = Encoding.UTF8.GetBytes(
                $"From: {userEmail}\r\n" +
                $"To: {email}\r\n" +
                $"Subject: {subject}\r\n" +
                "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                htmlMessage);

            var rawMessage = Convert.ToBase64String(plainTextBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");

            var gmailMessage = new Message
            {
                Raw = rawMessage
            };

            try
            {
                await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
                _logger.LogInformation("Email sent to {email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {email}", email);
                throw;
            }
        }
    }
}