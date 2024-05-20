using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands;

public class CustomerRegisterHandler : ICommandHandler<CustomerRegisterCommand, Result>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CustomerRegisterHandler(
        IJwtTokenService jwtTokenService, IAccountRepository accountRepository, IUnitOfWork unitOfWork, IMapper mapper
    )
    {
        _jwtTokenService = jwtTokenService;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
                return Result.Failure(new Error("400", "Account already existed."));
            }

            // 1.2 Update the account if the account is unverified.
            account.FirstName = request.FirstName;
            account.LastName = request.LastName;
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
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = BCrypUnitls.Hash(request.Password),
                AvatarUrl = "https://www.freecodecamp.org/news/content/images/2021/10/golang.png",
                RoleId = (int)Domain.Enums.Roles.Customer,
                AccountType = (int)AccountTypes.Local,
                Status = (int)AccountStatus.UnVerify
            };

            var refreshToken = _jwtTokenService.GenerateJwtRefreshToken(newAccount);

            newAccount.RefreshToken = refreshToken;
            return await CreateAccount(newAccount);
        }
    }

    private async Task<Result<Result>> CreateAccount(Account newAccount)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _accountRepository.AddAsync(newAccount);
            await _unitOfWork.CommitTransactionAsync();
            return Result.Success("Your account has been created successfully. Please verify it to proceed.");
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
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
            return Result.Success("Your account has been created successfully. Please verify it to proceed.");
        }
        catch (Exception e)
        {
            _unitOfWork.RollbackTransaction();
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }
}