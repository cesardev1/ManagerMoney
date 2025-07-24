using System.Net;
using System.Net.Mail;
using ManagerMoney.Models;

namespace ManagerMoney.Services;

public class EmailService : IEmailService
{
    private readonly SecretsOptions _secretsOptions;

    public EmailService(SecretsOptions secretsOptions)
    {
        _secretsOptions = secretsOptions;
    }

    public async Task SendEmailResetPassword(string to, string link)
    {
        var email = _secretsOptions.ConfigurationEmail_Email;
        var password = _secretsOptions.ConfigurationEmail_Password;
        var host = _secretsOptions.ConfigurationEmail_Host;
        var port = _secretsOptions.ConfigurationEmail_Port;

        var client = new SmtpClient(host, port);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        
        client.Credentials = new NetworkCredential(email, password);
        var from = email;
        var subject = "Reset Password";
        var body = $"Please click on the link to reset your password: {link}";
        var message = new MailMessage(from, to, subject, body);
        await client.SendMailAsync(message);
    }
}