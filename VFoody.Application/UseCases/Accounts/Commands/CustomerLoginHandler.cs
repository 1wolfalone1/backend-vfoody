using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Utils;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands;

public class CustomerLoginHandler : ICommandHandler<CustomerLoginCommand, Result>
{
    private IJwtTokenService _jwtTokenService;
    private IAccountRepository _accountRepository;
    private IMapper _mapper;

    public CustomerLoginHandler(IJwtTokenService jwtTokenService, IAccountRepository accountRepository, IMapper mapper)
    {
        _jwtTokenService = jwtTokenService;
        _accountRepository = accountRepository;
        _mapper = mapper;
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
        var accountResponse = new AccountResponse(); 
        this._mapper.Map(customerAccount, accountResponse);
        accountResponse.RoleName = Domain.Enums.Roles.Customer.GetDescription();

        return Result<LoginResponse>.Success(new LoginResponse
        {
            AccountResponse = accountResponse,
            AccessTokenResponse = new AccessTokenResponse
            {
                AccessToken = token,
                RefreshToken = token
            }
        });
    }
}