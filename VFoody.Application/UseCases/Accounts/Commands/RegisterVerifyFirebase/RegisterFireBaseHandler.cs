using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.RegisterVerifyFirebase;

public class RegisterFireBaseHandler : ICommandHandler<RegisterFireBaseCommand, Result>
{
    private readonly ILogger<RegisterFireBaseHandler> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAccountRepository _accountRepository;
    private readonly IFirebaseVerifyIDTokenService _firebaseVerifyIdTokenService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterFireBaseHandler(
        ILogger<RegisterFireBaseHandler> logger, IJwtTokenService jwtTokenService,
        IAccountRepository accountRepository, IUnitOfWork unitOfWork, IFirebaseVerifyIDTokenService firebaseVerifyIdTokenService)
    {
        _jwtTokenService = jwtTokenService;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _firebaseVerifyIdTokenService = firebaseVerifyIdTokenService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(RegisterFireBaseCommand request, CancellationToken cancellationToken)
    {
        FirebaseUser firebaseUser;
        try
        {
            firebaseUser = await _firebaseVerifyIdTokenService.GetFirebaseUser(request.Token);
        }
        catch (Exception e)
        {
            return Result.Failure(new Error("400", "Token không đúng"));
        }
        if (firebaseUser != null)
        {
            var account = _accountRepository.GetAccountByEmail(request.Email);
                    //1. Check existed account
                    if (account != null)
                    {
                        // 1.1 Return an error if the account exists and its status is not unverified.
                        if (account.Status != (int)AccountStatus.UnVerify)
                        {
                            return Result.Failure(new Error("400", "Email đã tồn tại."));
                        }

                        if (account.PhoneNumber != request.PhoneNumber && _accountRepository.CheckExistAccountByPhoneNumber(request.PhoneNumber))
                        {
                            return Result.Failure(new Error("400", "Số điện thoại đã tồn tại."));
                        }

                        account.PhoneNumber = request.PhoneNumber;
                        account.Password = BCrypUnitls.Hash(request.Password);
                        var refreshToken = _jwtTokenService.GenerateJwtRefreshToken(account);
                        account.RefreshToken = refreshToken;
                        return await UpdateAccount(account);
                    }
                    else
                    {
                        if (_accountRepository.CheckExistAccountByPhoneNumber(request.PhoneNumber))
                        {
                            return Result.Failure(new Error("400", "Số điện thoại đã tồn tại."));
                        }

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
        return Result.Failure(new Error("400", "Token không đúng"));
    }

    private async Task<Result<Result>> CreateAccount(Account account)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _accountRepository.AddAsync(account);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Create(new RegisterResponse
            {
                Email = account.Email,
                Message = "Your account has been created successfully."
            });
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }


    private async Task<Result<Result>> UpdateAccount(Account account)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            _accountRepository.Update(account);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Create(new RegisterResponse
            {
                Email = account.Email,
                Message = "Your account has been created successfully."
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