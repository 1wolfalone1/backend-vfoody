using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Firebases.Queries.GetFirebaseCustomToken;

public class GetFirebaseCustomTokenHandler : IQueryHandler<GetFirebaseCustomTokenQuery, Result>
{
    private readonly IFirebaseAuthenticateUserService _firebaseAuthenticate;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly ILogger<GetFirebaseCustomTokenHandler> _logger;

    public GetFirebaseCustomTokenHandler(IFirebaseAuthenticateUserService firebaseAuthenticate, ICurrentAccountService currentAccountService, ILogger<GetFirebaseCustomTokenHandler> logger)
    {
        _firebaseAuthenticate = firebaseAuthenticate;
        _currentAccountService = currentAccountService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetFirebaseCustomTokenQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var account = this._currentAccountService.GetCurrentAccount();
            var customToken = await this._firebaseAuthenticate.CreateCustomerToken(account.FUserId).ConfigureAwait(false);
            return Result.Success(new
            {
                CustomToken = customToken
            });
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            throw new InvalidBusinessException("Không thể lấy custom token từ firebase");
        }
    }
}