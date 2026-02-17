namespace BankRUs.Application.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string firstName, string accountNumber);
}