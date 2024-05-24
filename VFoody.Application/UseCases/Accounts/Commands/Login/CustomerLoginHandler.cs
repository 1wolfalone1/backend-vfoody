using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands;

public class CustomerLoginHandler : ICommandHandler<CustomerLoginCommand, Result>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerLoginHandler> _logger;

    public CustomerLoginHandler(IJwtTokenService jwtTokenService, IAccountRepository accountRepository, IMapper mapper, IUnitOfWork unitOfWork, ILogger<CustomerLoginHandler> logger)
    {
        _jwtTokenService = jwtTokenService;
        _accountRepository = accountRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(CustomerLoginCommand request, CancellationToken cancellationToken)
    {
        var customerAccount =
            this._accountRepository.GetCustomerAccount(request.AccountLogin.Email, BCrypUnitls.Hash(request.AccountLogin.Password));
        if (customerAccount == null)
            return Result.Failure(new Error("401", "Email or Password not correct"));
        
        if(customerAccount.RoleId != (int) Domain.Enums.Roles.Customer)
            return Result.Failure(new Error("401", "Your account not have permission to access this"));

        if (customerAccount.Status != (int)AccountStatus.Verify)
            return Result.Failure(new Error("401", "Your account haven't verify"));

        var token = this._jwtTokenService.GenerateJwtToken(customerAccount);
        var refreshToken = this._jwtTokenService.GenerateJwtRefreshToken(customerAccount);
        // Update token
        await this.UpdateRefreshToken(customerAccount, refreshToken).ConfigureAwait(false);
        
        var accountResponse = new AccountResponse(); 
        this._mapper.Map(customerAccount, accountResponse);
        accountResponse.RoleName = EnumHelper.GetEnumDescription<Domain.Enums.Roles>(customerAccount.RoleId);

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

    private async Task UpdateRefreshToken(Account account, string token)
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