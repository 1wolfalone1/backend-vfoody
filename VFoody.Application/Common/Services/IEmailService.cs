namespace VFoody.Application.Common.Services;

public interface IEmailService
{
    bool SendVerifyCode(string email, string code, int verifyType);
    bool SendEmail(string toEmail, string subject, string body);
}