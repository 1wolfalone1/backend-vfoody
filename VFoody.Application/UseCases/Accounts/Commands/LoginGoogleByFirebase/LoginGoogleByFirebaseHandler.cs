using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.LoginGoogleByFirebase;

public class LoginGoogleByFirebaseHandler : ICommandHandler<LoginGoogleByFirebaseCommand, Result>
{
    private readonly IFirebaseVerifyIDTokenService _firebaseVerify;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<LoginGoogleByFirebaseHandler> _logger;
    private readonly IMapper _mapper;

    public LoginGoogleByFirebaseHandler(IFirebaseVerifyIDTokenService firebaseVerify, IUnitOfWork unitOfWork, IAccountRepository accountRepository, IJwtTokenService jwtTokenService, ILogger<LoginGoogleByFirebaseHandler> logger, IMapper mapper)
    {
        _firebaseVerify = firebaseVerify;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(LoginGoogleByFirebaseCommand request, CancellationToken cancellationToken)
    {
        var firebaseUser = await this._firebaseVerify.GetFirebaseUser(request.IdToken);
        if (firebaseUser != null)
        {
            var account = this._accountRepository.GetAccountByEmail(firebaseUser.Email);
            if (account == default)
            {
                // Dont have account
                var newAccount = await this.RegisterANewCustomerAsync(firebaseUser).ConfigureAwait(false);
                return await this.GenerateJwtTokenAsync(newAccount).ConfigureAwait(false);
            }
            
            // Already have account
            return await this.GenerateJwtTokenAsync(account).ConfigureAwait(false);
        }
    
        return Result.Failure(new Error("404", "Id token không đúng"));
    }
    
    private async Task<Account> RegisterANewCustomerAsync(FirebaseUser userInfor)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var newAccount = new Account
            {
                Email = userInfor.Email,
                FirstName = string.Empty,
                LastName = userInfor.Name,
                Password = String.Empty,
                AvatarUrl = userInfor.Picture,
                RoleId = (int)Domain.Enums.Roles.Customer,
                AccountType = (int)AccountTypes.Google,
                Status = (int)AccountStatus.Verify,
                FUserId = userInfor.UserId,
            };

            await this._accountRepository.AddAsync(newAccount);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return newAccount;
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
            throw new ("Create account when login with google fail");
        }
    }
    
    private async Task<Result<LoginResponse>> GenerateJwtTokenAsync(Account acc)
    {
        var token = this._jwtTokenService.GenerateJwtToken(acc);
        var refreshToken = this._jwtTokenService.GenerateJwtRefreshToken(acc);
        // Update token
        await this.UpdateRefreshTokenAsync(acc, refreshToken).ConfigureAwait(false);
        
        var accountResponse = new AccountResponse(); 
        this._mapper.Map(acc, accountResponse);
        accountResponse.RoleName = Domain.Enums.Roles.Customer.GetDescription();

        return Result<LoginResponse>.Success(new LoginResponse
        {
            AccountResponse = accountResponse,
            AccessTokenResponse = new AccessTokenResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken
            }
        });   
    }
    
    private async Task UpdateRefreshTokenAsync(Account account, string token)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            account.RefreshToken = token;
            this._accountRepository.Update(account);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
        }
    }
}