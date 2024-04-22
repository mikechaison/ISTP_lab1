using MVC.Newsreel.Data;

namespace MVC.Newsreel.Services;

public interface IEmailService
{
    Task SendTestEmail(UserEmailOptions userEmailOptions);
    Task SendEmailForEmailConfirm(UserEmailOptions userEmailOptions);
    Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
}
