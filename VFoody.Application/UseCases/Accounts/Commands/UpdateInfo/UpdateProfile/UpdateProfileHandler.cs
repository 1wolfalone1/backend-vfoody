using System.Security.Authentication;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Exceptions.Base;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateInfo.UpdateProfile;

public class UpdateProfileHandler : ICommandHandler<UpdateProfileCommand, Result>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateProfileHandler> _logger;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IMapper _mapper;

    public UpdateProfileHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<UpdateProfileHandler> logger, ICurrentPrincipalService currentPrincipalService, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentPrincipalService = currentPrincipalService;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var email = this._currentPrincipalService.CurrentPrincipal;
            if (this._currentPrincipalService.CurrentPrincipalId != request.Id)
            {
                throw new AuthenticationException("Id của account và token không map");
            }
            
            var account = this._accountRepository.GetAccountByEmail(email);
            account.PhoneNumber = request.UpdateProfileRequest.PhoneNumber;
            account.LastName = request.UpdateProfileRequest.FullName;
            this._accountRepository.Update(account);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            var accounResponse = this._mapper.Map<AccountResponse>(account);

            return Result.Success(accounResponse);
        }
        catch (Exception e)
        {
            this._unitOfWork.RollbackTransaction();
            this._logger.LogError(e, e.Message);
            throw new Exception(e.Message);
        }
    }
}