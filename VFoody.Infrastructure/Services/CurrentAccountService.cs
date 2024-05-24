using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Services;

public class CurrentAccountService : ICurrentAccountService, IBaseService
{
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IAccountRepository _accountRepository;
    

    public CurrentAccountService(ICurrentPrincipalService currentPrincipalService, IAccountRepository accountRepository)
    {
        _currentPrincipalService = currentPrincipalService;
        _accountRepository = accountRepository;
    }

    public Account GetCurrentAccount()
    {
        var curerntAccountEmail = this._currentPrincipalService.CurrentPrincipal;
        return this._accountRepository.GetAccountByEmail(curerntAccountEmail);
    }
}