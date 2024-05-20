using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.ReVerify;

public class AccountReVerifyHandler : ICommandHandler<AccountReVerifyCommand, Result>
{
    private readonly ILogger<AccountReVerifyHandler> _logger;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public AccountReVerifyHandler(
        ILogger<AccountReVerifyHandler> logger,
        IVerificationCodeRepository verificationCodeRepository,
        IUnitOfWork unitOfWork, IAccountRepository accountRepository, IEmailService emailService)
    {
        _logger = logger;
        _verificationCodeRepository = verificationCodeRepository;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _emailService = emailService;
    }

    public async Task<Result<Result>> Handle(AccountReVerifyCommand request, CancellationToken cancellationToken)
    {
        var account = _accountRepository.GetAccountByEmail(request.Email);
        //1. Check existed account.
        if (account == null) return Result.Failure(new Error("400", "Not found email."));
        //2. Revoke old verification code
        var revokeSuccess = await RevokeVerificationCode(account.Id);
        if (!revokeSuccess)
        {
            return Result.Failure(new Error("500", "Internal server error."));
        }

        //3. Re create verification code
        var code = new Random().Next(1000, 10000).ToString();
        var isSendMail = SendVerifyCode(account.Email, code);
        if (!isSendMail)
        {
            return Result.Failure(new Error("500", "Internal server error."));
        }
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var verificationCode = new VerificationCode
            {
                AccountId = account.Id,
                Code = code,
                ExpiredTịme = DateTime.Now.AddMinutes(1),
                CodeType = (int)VerificationCodeTypes.Register,
                Status = (int)VerificationCodeStatus.Active
            };
            await _verificationCodeRepository.AddAsync(verificationCode);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Create(new ReVerifyResponse
            {
                Email = account.Email,
                Message = "Re-create verification code successfully. Check your email."
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }

    private bool SendVerifyCode(string email, string code)
    {
        return _emailService.SendEmail(email, "VFoody Account Verification Code",
            @"
                    <html>
                        <body style='font-family: Arial, sans-serif; color: #333;'>
                            <div style='margin-bottom: 20px; text-align: center;'>
                                <img src='https://www.freecodecamp.org/news/content/images/2021/10/golang.png' alt='VFoody Logo' style='display: block; margin: 0 auto;' />
                            </div>
                            <p>Hello,</p>
                            <p>Thank you for signing up for VFoody! Please use the following code to verify your account:</p>
                            <div style='text-align: center; margin: 20px;'>
                                <span style='font-size: 24px; padding: 10px; border: 1px solid #ccc;'>" + code + @"</span>
                            </div>
                            <p>If you did not sign up for an VFoody account, please ignore this email or contact our support team.</p>
                            <p>Best regards,</p>
                            <p>The VFoody Team</p>
                        </body>
                    </html>"
        );
    }

    private async Task<bool> RevokeVerificationCode(int accountId)
    {
        var verificationCodes = _verificationCodeRepository.FindByAccountIdAndCodeTypeAndStatus(
            accountId,
            (int)VerificationCodeTypes.Register,
            (int)VerificationCodeStatus.Active
        ).ToList();
        if (verificationCodes.Count <= 0) return true;
        verificationCodes.ForEach(code => code.Status = (int)VerificationCodeStatus.Revoked);
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            _verificationCodeRepository.UpdateRange(verificationCodes);
            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return false;
        }
    }
}