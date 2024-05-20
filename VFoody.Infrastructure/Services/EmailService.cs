using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using VFoody.Application.Common.Services;

namespace VFoody.Infrastructure.Services;

public class EmailService : IEmailService, IBaseService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            var fromMail = _configuration["EMAIL"] ?? "";
            var fromPassword = _configuration["EMAIL_PASSWORD"] ?? "";

            var message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(toEmail));
            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient(_configuration["SMTP_CLIENT"] ?? "")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
            return true;
        } catch (Exception ex)
        {
            return false;
        }        
    }
}
