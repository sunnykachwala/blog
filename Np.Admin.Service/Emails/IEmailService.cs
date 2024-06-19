namespace Np.Admin.Service.Emails
{
    using Np.ViewModel;
    public interface IEmailService
    {
        bool SendEmail(EmailDto emailDto, SMTPSettings smtpSettings, EmailSettings emailSettings);
    }
}
