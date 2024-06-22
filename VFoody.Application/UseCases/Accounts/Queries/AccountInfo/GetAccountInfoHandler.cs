using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Utils;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Queries.AccountInfo;

public class GetAccountInfoHandler : IQueryHandler<GetAccountInfoQuery, Result>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<GetAccountInfoHandler> _logger;
    private readonly IMapper _mapper;


    public GetAccountInfoHandler(IAccountRepository accountRepository, ILogger<GetAccountInfoHandler> logger, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
    {
        var account = _accountRepository.GetByIdIncludeBuilding(request.accountId);
        if (account == null)
        {
            Result.Failure(new Error("400", "Not found this account."));
        }

        var response = _mapper.Map<AccountInfoResponse>(account);
        response.RoleName = EnumHelper.GetEnumDescription<Domain.Enums.Roles>(account!.RoleId);
        return Result.Success(response);
    }
}