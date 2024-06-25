using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateAccountDeviceToken;

public class UpdateAccountDeviceTokenHandler : ICommandHandler<UpdateAccountDeviceTokenCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;

    public UpdateAccountDeviceTokenHandler(IUnitOfWork unitOfWork, IAccountRepository accountRepository, ICurrentPrincipalService currentPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _currentPrincipalService = currentPrincipalService;
    }

    public async Task<Result<Result>> Handle(UpdateAccountDeviceTokenCommand request, CancellationToken cancellationToken)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var account = this._accountRepository.GetAccountByEmail(this._currentPrincipalService.CurrentPrincipal);
            account.DeviceToken = request.DeviceToken;
            this._accountRepository.Update(account);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return Result.Success("Cập nhật device token thành công");
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            throw e;
        }
    }
}