using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Firebases.Commands.VerifyIdTokens;

public class VerifyIDTokensHandler : ICommandHandler<VerifyIDTokensCommand, Result>
{
    private readonly IFirebaseVerifyIDTokenService _firebaseVerify;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAccountRepository _accountRepository;

    public VerifyIDTokensHandler(IFirebaseVerifyIDTokenService firebaseVerify, IJwtTokenService jwtTokenService, IAccountRepository accountRepository)
    {
        _firebaseVerify = firebaseVerify;
        _jwtTokenService = jwtTokenService;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Result>> Handle(VerifyIDTokensCommand request, CancellationToken cancellationToken)
    {
        var firebaseUser = await this._firebaseVerify.GetFirebaseUser(request.IdToken);
        if (firebaseUser != null)
        {
            var phoneNumber = firebaseUser.PhoneNumber.Replace("+84", "0");
            var account = await this._accountRepository.GetAccountByPhoneNumber(phoneNumber).ConfigureAwait(false);

            if (account == null)
            {
                return Result.Failure(new Error("404",
                    "Không tìm được account với số điện thoại " + phoneNumber));
            }

            var jwtToken = _jwtTokenService.GenerateJwtToken(account);
            return Result.Success(jwtToken);
        }
    
        return Result.Failure(new Error("404", "Id token không đúng"));

    }
}