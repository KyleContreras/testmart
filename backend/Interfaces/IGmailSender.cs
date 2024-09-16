namespace backend.Interfaces;

public interface IGmailSender
{
    public Task SendConfirmationEmailAsync(string email, string confirmationLink);
}