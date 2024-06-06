using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.ForgotPasswordFirebase;

public class ForgotPasswordFirebaseHandler : ICommandHandler<ForgotPasswordFirebaseCommand, Result>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ForgotPasswordFirebaseHandler> _logger;
    private readonly ICurrentPrincipalService _currentPrincipal;

    public ForgotPasswordFirebaseHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ICurrentPrincipalService currentPrincipal)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _currentPrincipal = currentPrincipal;
    }

    public async Task<Result<Result>> Handle(ForgotPasswordFirebaseCommand request, CancellationToken cancellationToken)
    {
        var email = this._currentPrincipal.CurrentPrincipal;
        var account = this._accountRepository.GetAccountByEmail(email);
        if (account != null)
        {
            await UpdateNewPassword(account, request.ForgotPasswordFirebaseRequest.NewPassword).ConfigureAwait(false);
            return Result.Success("Đổi mật khẩu thành công");
        }

        throw new Exception("Không tìm được account với email " + email);

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
}