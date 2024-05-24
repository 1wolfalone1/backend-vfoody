using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
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

    public GetCustomerInforHandler(ICurrentAccountService currentAccountService, ILogger<GetCustomerInforHandler> logger, IMapper mapper)
    {
        _currentAccountService = currentAccountService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetCustomerInforQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var account = this._currentAccountService.GetCurrentAccount();
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