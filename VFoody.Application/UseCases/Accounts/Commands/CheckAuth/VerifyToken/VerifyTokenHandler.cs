using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.CheckAuth.VerifyToken;

public class VerifyTokenHandler : ICommandHandler<VerifyTokenCommand, Result>
{
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public VerifyTokenHandler(ICurrentAccountService currentAccountService, IMapper mapper)
    {
        _currentAccountService = currentAccountService;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(VerifyTokenCommand request, CancellationToken cancellationToken)
    {
        var account = this._currentAccountService.GetCurrentAccount();
        var verifyResponse = this._mapper.Map<VerifyTokenResponse>(account);
        return Result.Success(verifyResponse);
    }
}