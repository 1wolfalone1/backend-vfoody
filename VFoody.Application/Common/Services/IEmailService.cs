namespace VFoody.Application.Common.Services;

public interface IEmailService
{
    bool SendEmail(string toEmail, string subject, string body);
}