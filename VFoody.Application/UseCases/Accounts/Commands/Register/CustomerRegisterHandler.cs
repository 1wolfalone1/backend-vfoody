using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.Register;

public class CustomerRegisterHandler : ICommandHandler<CustomerRegisterCommand, Result>
{
    private readonly ILogger<CustomerRegisterHandler> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAccountRepository _accountRepository;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerRegisterHandler(
        ILogger<CustomerRegisterHandler> logger, IJwtTokenService jwtTokenService,
        IAccountRepository accountRepository, IUnitOfWork unitOfWork,
        IVerificationCodeRepository verificationCodeRepository, IEmailService emailService)
    {
        _jwtTokenService = jwtTokenService;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _verificationCodeRepository = verificationCodeRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(CustomerRegisterCommand request, CancellationToken cancellationToken)
    {
        var account = _accountRepository.GetAccountByEmail(request.Email);
        //1. Check existed account
        if (account != null)
        {
            // 1.1 Return an error if the account exists and its status is not unverified.
            if (account.Status != (int)AccountStatus.UnVerify)
            {
                return Result.Failure(new Error("400", "Tài khoản đã tồn tại."));
            }
            // 1.2 Update the account if the account is unverified.
            // 1.2.1 Revoke verification code
            var revokeSuccess = await RevokeVerificationCode(account.Id);
            if (!revokeSuccess)
            {
                return Result.Failure(new Error("500", "Internal server error."));
            }

            account.PhoneNumber = request.PhoneNumber;
            account.Password = BCrypUnitls.Hash(request.Password);
            var refreshToken = _jwtTokenService.GenerateJwtRefreshToken(account);
            account.RefreshToken = refreshToken;
            return await UpdateAccount(account);
        }
        else
        {
            //1.3 Create a new account.
            var newAccount = new Account
            {
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Password = BCrypUnitls.Hash(request.Password),
                AvatarUrl = "https://www.freecodecamp.org/news/content/images/2021/10/golang.png",
                FirstName = "",
                LastName = "",
                RoleId = (int)Domain.Enums.Roles.Customer,
                AccountType = (int)AccountTypes.Local,
                Status = (int)AccountStatus.UnVerify
            };

            var refreshToken = _jwtTokenService.GenerateJwtToken(newAccount);

            newAccount.RefreshToken = refreshToken;
            return await CreateAccount(newAccount);
        }
    }

    private async Task<Result<Result>> CreateAccount(Account account)
    {
        var code = new Random().Next(1000, 10000).ToString();
        var isSendMail = _emailService.SendVerifyCode(account.Email, code, (int)VerificationCodeTypes.Register);
        if (!isSendMail)
        {
            return Result.Failure(new Error("500", "Internal server error."));
        }
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _accountRepository.AddAsync(account);
            var verificationCode = new VerificationCode
            {
                Account = account,
                Code = code,
                ExpiredTịme = DateTime.Now.AddMinutes(3),
                CodeType = (int)VerificationCodeTypes.Register,
                Status = (int)VerificationCodeStatus.Active
            };
            await _verificationCodeRepository.AddAsync(verificationCode);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Create(new RegisterResponse
            {
                Email = account.Email,
                Message = "Your account has been created successfully. Please check your email to get the verification code and proceed."
            });
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }

    private async Task<bool> RevokeVerificationCode(int accountId)
    {
        var verificationCodes = _verificationCodeRepository.FindByAccountIdAndCodeTypeAndStatus(
            accountId,
            (int)VerificationCodeTypes.Register,
            (int)VerificationCodeStatus.Active
        ).ToList();
        if (verificationCodes.Count <= 0) return true;
        verificationCodes.ForEach(code => code.Status = (int)VerificationCodeStatus.Revoked);
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            _verificationCodeRepository.UpdateRange(verificationCodes);
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

    private async Task<Result<Result>> UpdateAccount(Account account)
    {
        var code = new Random().Next(1000, 10000).ToString();
        var isSendMail = _emailService.SendVerifyCode(account.Email, code, (int)VerificationCodeTypes.Register);
        if (!isSendMail)
        {
            return Result.Failure(new Error("500", "Internal server error."));
        }
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            _accountRepository.Update(account);
            var verificationCode = new VerificationCode
            {
                AccountId = account.Id,
                Code = code,
                ExpiredTịme = DateTime.Now.AddMinutes(3),
                CodeType = (int)VerificationCodeTypes.Register,
                Status = (int)VerificationCodeStatus.Active
            };
            await _verificationCodeRepository.AddAsync(verificationCode);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Create(new RegisterResponse
            {
                Email = account.Email,
                Message = "Your account has been created successfully. Please check your email to get the verification code and proceed."
            });
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }
}