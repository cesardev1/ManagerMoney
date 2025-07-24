namespace ManagerMoney.Services;

public interface IEmailService
{
    Task SendEmailResetPassword(string to, string link);
}