using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Accounts.Commands.Verify;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.VerifyForgotPasswordCode;

public class VerifyCodeForgotPasswordHandler : ICommandHandler<VerifyCodeForgotPasswordCommand, Result>
{
    private readonly IVerificationCodeRepository _verificationCodeRepository;

    public VerifyCodeForgotPasswordHandler(IVerificationCodeRepository verificationCodeRepository)
    {
        _verificationCodeRepository = verificationCodeRepository;
    }

    public async Task<Result<Result>> Handle(VerifyCodeForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        //1. Check existed verification code
        var verificationCode = _verificationCodeRepository.FindByCodeAndStatusAndEmail(
            request.Code.ToString(), (int)VerificationCodeTypes.ForgotPassword, (int)VerificationCodeStatus.Active, request.Email
        );
        if (verificationCode == null)
        {
            // 1.1 Response not found verification code
            return Result.Failure(new Error("400", "Not correct verification code."));
        }
        else
        {
            if (verificationCode.ExpiredTịme < DateTime.Now)
            {
                return Result.Failure(new Error("400", "Verification code expired."));
            }

            return Result.Success("The verification code is correct.");
        }

    }
}