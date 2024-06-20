using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Queries.ManageAccount;

public class GetAllAccountHandler : IQueryHandler<GetAllAccountQuery, Result>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<GetAllAccountHandler> _logger;
    private readonly IMapper _mapper;

    public GetAllAccountHandler(
        IAccountRepository accountRepository, ILogger<GetAllAccountHandler> logger, IMapper mapper
    )
    {
        _accountRepository = accountRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
    {
        var accounts = _accountRepository.GetAll(
            (int)Domain.Enums.Roles.Admin, request.PageIndex, request.PageSize
        );
        var totalAccounts = _accountRepository.CountAll((int)Domain.Enums.Roles.Admin);
        var result = new PaginationResponse<ManageAccountResponse>(_mapper.Map<List<ManageAccountResponse>>(accounts),
            request.PageIndex, request.PageSize, totalAccounts);
        return Result.Success(result);
    }
}