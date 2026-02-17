using BankRUs.Application.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace BankRUs.Intrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string firstName, string accountNumber)
    {
        var smtpHost = _configuration["Email:SmtpHost"] ?? "localhost";
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "25");
        var fromEmail = _configuration["Email:FromEmail"] ?? "noreply@bankrus.se";
        var fromName = _configuration["Email:FromName"] ?? "BankRUs";

        using var smtpClient = new SmtpClient(smtpHost, smtpPort);

        // För Smtp4dev behövs ingen autentisering
        smtpClient.Credentials = null;
        smtpClient.EnableSsl = false;

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = "Välkommen till BankRUs!",
            Body = $@"
            Hej {firstName}!

            Välkommen till BankRUs! Ditt konto är nu skapat och redo att användas.

            Ditt kontonummer: {accountNumber}

            Vi ser fram emot att hjälpa dig med dina bankärenden.

            Med vänliga hälsningar,
            BankRUs Team
            ",
            IsBodyHtml = false
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}