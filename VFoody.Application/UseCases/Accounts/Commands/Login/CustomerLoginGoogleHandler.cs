using System.Net.Http.Headers;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Enums;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands;

public class CustomerLoginGoogleHandler : ICommandHandler<CustomerLoginGoogleCommand, Result>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerLoginGoogleHandler> _logger;
    private readonly IConfiguration _configuration;
    private readonly IFirebaseAuthenticateUserService _firebaseAuthenticate;

    public CustomerLoginGoogleHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IMapper mapper, ILogger<CustomerLoginGoogleHandler> logger, IConfiguration configuration, IFirebaseAuthenticateUserService firebaseAuthenticate)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
        _firebaseAuthenticate = firebaseAuthenticate;
    }

    public async Task<Result<Result>> Handle(CustomerLoginGoogleCommand request, CancellationToken cancellationToken)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);
        
        var response = await client.GetAsync(_configuration["API_GET_USER_INFO_GOOGLE"]).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var userInfor = JsonConvert.DeserializeObject<GoogleUserInfor>(responseContent);

        var accTemp = this._accountRepository.GetAccountWithBuildingByEmail(userInfor.Email);
        // With email already regis account
        if (accTemp != null)
        {
            if (accTemp.Status == (int)AccountStatus.UnVerify)
            {
                return Result.Failure(new Error("400", ResponseCode.VerifyErrorInvalidAccount.GetDescription()));
            }

            if (accTemp.Status == (int)AccountStatus.Ban)
            {
                return Result.Failure(new Error("400", ResponseCode.BanErrorAccount.GetDescription()));
            }

            return await this.GenerateJwtTokenAsync(accTemp).ConfigureAwait(false);
        }

        // With new account
        Account acc = await this.RegisterANewCustomer(userInfor).ConfigureAwait(false);
        return await this.GenerateJwtTokenAsync(acc).ConfigureAwait(false);
    }
    

    private async Task<Account> RegisterANewCustomer(GoogleUserInfor userInfor)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            // Create an user in firebase and save firebase UId to account
            string firebaseUid = string.Empty;
            try
            {
                firebaseUid = await this._firebaseAuthenticate.CreateUser(userInfor.Email,
                    null,
                    null,
                    userInfor.Name,
                    userInfor.Picture).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (firebaseUid != string.Empty)
                {
                    await this._firebaseAuthenticate.DeleteUserAccount(firebaseUid).ConfigureAwait(false);
                }
                this._logger.LogError(e, e.Message);
            }

            var building = new Building();
            building.Name = string.Empty;
        
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
                FUserId = firebaseUid,
                Building = building
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
}