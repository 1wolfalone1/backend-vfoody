using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Queries;

public class GetAllAccountHandler : IQueryHandler<GetAllAccountQuery, Result<List<Account>>>
{
    private readonly IAccountRepository accountRepository;
    private readonly IDapperService dapperService;
    private readonly ITestService testService;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly ILogger<GetAllAccountHandler> _logger;

    public GetAllAccountHandler(IAccountRepository accountRepository, IDapperService dapperService, ITestService testService, ICurrentPrincipalService currentPrincipalService, ILogger<GetAllAccountHandler> logger)
    {
        this.accountRepository = accountRepository;
        this.dapperService = dapperService;
        this.testService = testService;
        _currentPrincipalService = currentPrincipalService;
        _logger = logger;
    }


    public async Task<Result<Result<List<Account>>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
    {
        testService.TestWriteLog();
        this._logger.LogInformation($"id {this._currentPrincipalService.CurrentPrincipalId}");
        return Result.Success(await this.accountRepository.Get(acc => acc != null).ToListAsync<Account>());
    }
}
