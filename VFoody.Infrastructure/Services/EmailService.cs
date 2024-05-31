using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using VFoody.Application.Common.Services;
using VFoody.Domain.Enums;

namespace VFoody.Infrastructure.Services;

public class EmailService : IEmailService, IBaseService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool SendVerifyCode(string email, string code, int verifyType)
    {
        if (verifyType == (int)VerificationCodeTypes.Register)
        {
            return SendEmail(email, "Mã Xác Thực Tài Khoản VFoody",
                @"
                    <html>
                        <body style='font-family: Arial, sans-serif; color: #333;'>
                            <div style='margin-bottom: 20px; text-align: center;'>
                                <img src='https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1717170673218-42e3e4c6-ff37-4810-b6ab-860551bba3b7' alt='VFoody Logo' style='display: block; margin: 0 auto;' />
                            </div>
                            <p>Xin chào,</p>
                            <p>Cảm ơn bạn đã đăng ký VFoody! Vui lòng sử dụng mã sau để xác thực tài khoản của bạn:</p>
                            <div style='text-align: center; margin: 20px;'>
                                <span style='font-size: 24px; padding: 10px; border: 1px solid #ccc;'>" + code + @"</span>
                            </div>
                            <p>Nếu bạn không đăng ký tài khoản VFoody, vui lòng bỏ qua email này hoặc liên hệ với đội hỗ trợ của chúng tôi.</p>
                            <p>Trân trọng,</p>
                            <p>Đội ngũ VFoody</p>
                        </body>
                    </html>"
            );
        }

        if (verifyType == (int)VerificationCodeTypes.ForgotPassword)
        {
            return SendEmail(email, "Mã Quên Mật Khẩu Tài Khoản VFoody",
                @"
                    <html>
                        <body style='font-family: Arial, sans-serif; color: #333;'>
                            <div style='margin-bottom: 20px; text-align: center;'>
                                <img src='https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1717170673218-42e3e4c6-ff37-4810-b6ab-860551bba3b7' alt='VFoody Logo' style='display: block; margin: 0 auto;' />
                            </div>
                            <p>Xin chào,</p>
                            <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản VFoody của bạn. Vui lòng sử dụng mã sau để đặt lại mật khẩu của bạn:</p>
                            <div style='text-align: center; margin: 20px;'>
                                <span style='font-size: 24px; padding: 10px; border: 1px solid #ccc;'>" + code + @"</span>
                            </div>
                            <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này hoặc liên hệ với đội hỗ trợ của chúng tôi.</p>
                            <p>Trân trọng,</p>
                            <p>Đội ngũ VFoody</p>
                        </body>
                    </html>"
            );
        }

        return true;
    }

    private bool SendEmail(string toEmail, string subject, string body)
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
