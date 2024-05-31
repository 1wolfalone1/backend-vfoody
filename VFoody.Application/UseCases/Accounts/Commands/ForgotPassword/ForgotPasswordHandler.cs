using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Utils;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.ForgotPassword;

public class ForgotPasswordHandler : ICommandHandler<ForgotPasswordCommand, Result>
{
    private readonly ILogger<ForgotPasswordHandler> _logger;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ForgotPasswordHandler(
        ILogger<ForgotPasswordHandler> logger,
        IVerificationCodeRepository verificationCodeRepository,
        IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _verificationCodeRepository = verificationCodeRepository;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        //1. Check existed verification code
        var verificationCode = _verificationCodeRepository.FindByCodeAndStatusAndEmail(
            request.Code.ToString(), (int)VerificationCodeTypes.ForgotPassword, (int)VerificationCodeStatus.Active, request.Email
        );
        if (verificationCode == null)
        {
            // 1.1 Response not found verification code
            return Result.Failure(new Error("400", "Mã xác minh không chính xác."));
        }

        //1.2 Revoke verification code, check expired time and update new password
        //1.2.1 Revoke verification code
        var revokeSuccess = await RevokeVerificationCode(verificationCode);
        if (!revokeSuccess)
        {
            return Result.Failure(new Error("500", "Internal server error."));
        }

        //1.2.2 Check expired time of code
        if (verificationCode.ExpiredTịme < DateTime.Now)
        {
            return Result.Failure(new Error("400", "Mã xác minh đã hết hạn."));
        }

        //1.2.3 Update account status
        var account = _accountRepository.GetAccountByEmail(request.Email)!;
        var updateSuccess = await UpdateNewPassword(account, request.NewPassword);
        return !updateSuccess ? Result.Failure(new Error("500", "Internal server error.")) : Result.Success("Update new password successfully.");
    }

    private async Task<bool> UpdateNewPassword(Account account, string newPassword)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            account.Password = BCrypUnitls.Hash(newPassword);
            _accountRepository.Update(account);
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

    private async Task<bool> RevokeVerificationCode(VerificationCode verificationCode)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            verificationCode.Status = (int)VerificationCodeStatus.Revoked;
            _verificationCodeRepository.Update(verificationCode);
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