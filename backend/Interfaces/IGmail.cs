namespace backend.Interfaces;

public interface IGmail
{
    public Task SendConfirmationEmailAsync(string email, string confirmationLink);
}