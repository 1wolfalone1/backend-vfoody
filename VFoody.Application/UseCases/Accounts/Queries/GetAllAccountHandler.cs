﻿using MediatR;
using Microsoft.EntityFrameworkCore;
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
    private readonly IAccountService accountService;
    private readonly ITestService testService;

    public GetAllAccountHandler(IAccountRepository accountRepository, IDapperService dapperService, IAccountService accountService, ITestService testService)
    {
        this.accountRepository = accountRepository;
        this.dapperService = dapperService;
        this.accountService = accountService;
        this.testService = testService;
    }


    public async Task<Result<Result<List<Account>>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
    {
        var test = this.dapperService.SingleOrDefault<int>(QueryName.TestQuery, null);
        var test1 = await this.dapperService.SingleOrDefaultAsync<int>(QueryName.TestQuery, null).ConfigureAwait(false);
        accountService.TestWriteLog();
        testService.TestWriteLog();
        return Result.Success(await this.accountRepository.Get(acc => acc != null).ToListAsync<Account>());
    }
}
