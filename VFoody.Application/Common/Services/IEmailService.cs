namespace VFoody.Application.Common.Services;

public interface IEmailService
{
    bool SendVerifyCode(string email, string code, int verifyType);
}