using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.CheckAccount;

public class CheckAccountHandler : ICommandHandler<CheckAccountCommand, Result>
{
    private readonly IAccountRepository _accountRepository;

    public CheckAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<Result>> Handle(CheckAccountCommand request, CancellationToken cancellationToken)
    {
        var account = _accountRepository.GetAccountByEmail(request.Email);
        //1. Check existed account
        if (account != null)
        {
            // 1.1 Return an error if the account exists and its status is not unverified.
            if (account.Status != (int)AccountStatus.UnVerify)
            {
                return Result.Failure(new Error("400", "Email đã tồn tại."));
            }

            // 1.2 Return an error if the phone number is duplicated.
            if (account.PhoneNumber != request.PhoneNumber &&
                _accountRepository.CheckExistAccountByPhoneNumber(request.PhoneNumber))
            {
                return Result.Failure(new Error("400", "Số điện thoại đã tồn tại."));
            }
        }
        else
        {
            // 1.3 Account not found with this email. An error will be returned if the phone number is duplicated.
            if (_accountRepository.CheckExistAccountByPhoneNumber(request.PhoneNumber))
            {
                return Result.Failure(new Error("400", "Số điện thoại đã tồn tại."));
            }
        }

        return Result.Success("Không tồn tại tài khoản");
    }
}