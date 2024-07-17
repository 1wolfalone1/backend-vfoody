using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Commands.VerifyRegisterCode;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.Verify;

public class AccountVerifyHandler : ICommandHandler<AccountVerifyCommand, Result>
{
    private readonly ILogger<AccountVerifyHandler> _logger;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseAuthenticateUserService _firebaseAuthenticate;

    public AccountVerifyHandler(
        ILogger<AccountVerifyHandler> logger,
        IVerificationCodeRepository verificationCodeRepository,
        IUnitOfWork unitOfWork, IAccountRepository accountRepository,
        IJwtTokenService jwtTokenService, IMapper mapper,
        IFirebaseAuthenticateUserService firebaseAuthenticate)
    {
        _logger = logger;
        _verificationCodeRepository = verificationCodeRepository;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _firebaseAuthenticate = firebaseAuthenticate;
    }

    public async Task<Result<Result>> Handle(AccountVerifyCommand request, CancellationToken cancellationToken)
    {
        //1. Check existed verification code
        var verificationCode = _verificationCodeRepository.FindByCodeAndStatusAndEmail(
            request.Code.ToString(), (int)VerificationCodeTypes.Register, (int)VerificationCodeStatus.Active, request.Email
        );
        if (verificationCode == null)
        {
            // 1.1 Response not found verification code
            return Result.Failure(new Error("400", "Mã xác minh không chính xác."));
        }

        //1.2 Revoke verification code, check expired time and update account status
        //1.2.1 Revoke verification code
        var revokeSuccess = await RevokeVerificationCode(verificationCode);
        if (!revokeSuccess)
        {
            return Result.Failure(new Error("500", "Internal server error."));
        }
        //1.2.2 Check expired time of code
        if (verificationCode.ExpiredTịme < DateTime.Now)
        {
            return Result.Failure(new Error("400", "Mã xác minh đã hết hạn."));
        }

        //1.2.3 Update account status
        var account = _accountRepository.GetAccountByEmail(request.Email)!;
        var updateSuccess = await UpdateAccountStatus(account);
        if (!updateSuccess)
        {
            return Result.Failure(new Error("500", "Internal server error."));
        }
        
        // 1.2.3.4 Update firebase Uid
        string firebaseUid = string.Empty;
        try
        {
            firebaseUid = await this._firebaseAuthenticate.CreateUser(request.Email, null,
                null, account.LastName, null);
            this.UpdateAccountFirebaseUid(account, firebaseUid);
            this._logger.LogInformation($"FirebaseUid In verify token {firebaseUid}");
        }
        catch (Exception e)
        {
            if (firebaseUid != string.Empty)
            {
                await this._firebaseAuthenticate.DeleteUserAccount(firebaseUid);
            }
            this._logger.LogError(e, e.Message);
        }

        //1.2.5 Response
        var accountResponse = new AccountResponse();
        var token = _jwtTokenService.GenerateJwtToken(account);
        _mapper.Map(account, accountResponse);
        accountResponse.RoleName = Domain.Enums.Roles.Customer.GetDescription();
        return Result.Success(new LoginResponse
        {
            AccountResponse = accountResponse,
            AccessTokenResponse = new AccessTokenResponse
            {
                AccessToken = token,
                RefreshToken = account.RefreshToken!
            }
        });
    }

    private async Task<bool> UpdateAccountFirebaseUid(Account account, string uid)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var accountUpdate = this._accountRepository.GetAccountByEmail(account.Email);
            accountUpdate.FUserId = uid;
            _accountRepository.Update(accountUpdate);
            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return false;
        }
    }
    
    private async Task<bool> UpdateAccountStatus(Account account)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            account.Status = (int)AccountStatus.Verify;
            _accountRepository.Update(account);
            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return false;
        }
    }

    private async Task<bool> RevokeVerificationCode(VerificationCode verificationCode)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            verificationCode.Status = (int)VerificationCodeStatus.Revoked;
            _verificationCodeRepository.Update(verificationCode);
            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return false;
        }
    }
}