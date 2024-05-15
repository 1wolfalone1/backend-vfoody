using MediatR;
using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Domain.Entities;

namespace VFoody.Application.UseCases.Accounts.Queries;

public class GetAllAccountHandler : IRequestHandler<GetAllAccounQuery, List<Account>>
{
    private readonly IAccountRepository accountRepository;
    private readonly IDapperService dapperService;

    public GetAllAccountHandler(IAccountRepository accountRepository, IDapperService dapperService)
    {
        this.accountRepository = accountRepository;
        this.dapperService = dapperService;
    }

    public async Task<List<Account>> Handle(GetAllAccounQuery request, CancellationToken cancellationToken)
    {
        var test = this.dapperService.SingleOrDefault<int>(QueryName.TestQuery, null);
        return await this.accountRepository.Get(acc => acc != null).ToListAsync<Account>();
    }
}
