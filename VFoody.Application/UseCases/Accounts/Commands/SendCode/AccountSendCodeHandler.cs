using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.SendCode;

public class AccountSendCodeHandler : ICommandHandler<AccountSendCodeCommand, Result>
{
    private readonly ILogger<AccountSendCodeHandler> _logger;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public AccountSendCodeHandler(
        ILogger<AccountSendCodeHandler> logger,
        IVerificationCodeRepository verificationCodeRepository,
        IUnitOfWork unitOfWork, IAccountRepository accountRepository, IEmailService emailService)
    {
        _logger = logger;
        _verificationCodeRepository = verificationCodeRepository;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _emailService = emailService;
    }

    public async Task<Result<Result>> Handle(AccountSendCodeCommand request, CancellationToken cancellationToken)
    {
        var account = _accountRepository.GetAccountByEmail(request.Email);
        //1. Check existed account.
        if (account == null) return Result.Failure(new Error("400", "Không tìm thấy email."));
        //2. Revoke old verification code
        var revokeSuccess = await RevokeVerificationCode(account.Id, request.VerifyType);
        if (!revokeSuccess)
        {
            return Result.Failure(new Error("500", "Internal server error."));
        }

        //3. Re create verification code
        var code = new Random().Next(1000, 10000).ToString();
        var isSendMail = _emailService.SendVerifyCode(account.Email, code, request.VerifyType);
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
                ExpiredTịme = DateTime.Now.AddMinutes(3),
                Status = (int)VerificationCodeStatus.Active
            };

            verificationCode.CodeType = request.VerifyType switch
            {
                (int)VerificationCodeTypes.Register => (int)VerificationCodeTypes.Register,
                (int)VerificationCodeTypes.ForgotPassword => (int)VerificationCodeTypes.ForgotPassword,
                _ => verificationCode.CodeType
            };

            await _verificationCodeRepository.AddAsync(verificationCode);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Create(new ReVerifyResponse
            {
                Email = account.Email,
                Message = "Create verification code successfully. Check your email."
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }

    private async Task<bool> RevokeVerificationCode(int accountId, int verifyType)
    {
        var codeType = verifyType switch
        {
            (int)VerificationCodeTypes.Register => (int)VerificationCodeTypes.Register,
            (int)VerificationCodeTypes.ForgotPassword => (int)VerificationCodeTypes.ForgotPassword,
            _ => verifyType
        };

        var verificationCodes = _verificationCodeRepository.FindByAccountIdAndCodeTypeAndStatus(
            accountId,
            codeType,
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