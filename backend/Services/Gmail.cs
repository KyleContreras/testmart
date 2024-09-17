using backend.Interfaces;
using MimeKit;

namespace backend.Services;

public class Gmail : IGmail
{
    private readonly Google.Apis.Gmail.v1.GmailService _gmailService;

    public Gmail(Google.Apis.Gmail.v1.GmailService gmailService)
    {
        _gmailService = gmailService;
    }

    // Method to send the account verification email
    public async Task SendConfirmationEmailAsync(string email, string confirmationLink)
    {
        string subject = "Confirm your email";
        string body = $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";
        
        var emailContent = CreateBase64UrlEmail(email, subject, body);
        var message = new Google.Apis.Gmail.v1.Data.Message
        {
            Raw = emailContent
        };

        await _gmailService.Users.Messages.Send(message, "me").ExecuteAsync();
    }

    // Helper method to create and encode the email
    private string CreateBase64UrlEmail(string to, string subject, string body)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress("Your Name", "your-email@gmail.com"));
        mailMessage.To.Add(new MailboxAddress("", to));
        mailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = body };
        mailMessage.Body = bodyBuilder.ToMessageBody();

        using (var stream = new MemoryStream())
        {
            mailMessage.WriteTo(stream);
            return Convert.ToBase64String(stream.ToArray())
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}