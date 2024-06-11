using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Queries;

public class GetCustomerInforHandler : IQueryHandler<GetCustomerInforQuery, Result>
{
    private readonly ICurrentAccountService _currentAccountService;
    private readonly ILogger<GetCustomerInforHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ICurrentPrincipalService _currentPrincipal;
    private readonly IAccountRepository _accountRepository;

    public GetCustomerInforHandler(ICurrentAccountService currentAccountService, ILogger<GetCustomerInforHandler> logger, IMapper mapper, ICurrentPrincipalService currentPrincipal, IAccountRepository accountRepository)
    {
        _currentAccountService = currentAccountService;
        _logger = logger;
        _mapper = mapper;
        _currentPrincipal = currentPrincipal;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Result>> Handle(GetCustomerInforQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var account = this._accountRepository.GetAccountWithBuildingByEmail(this._currentPrincipal.CurrentPrincipal);
            var accountResponse = new AccountResponse();
            this._mapper.Map(account, accountResponse);
            accountResponse.RoleName = EnumHelper.GetEnumDescription<Domain.Enums.Roles>(account.RoleId);

            return Result.Success(accountResponse);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            throw new Exception(e.Message);
        }
    }
}