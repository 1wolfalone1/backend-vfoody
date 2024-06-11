using System.Security.Authentication;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Accounts.Commands.UpdateInfo.UpdateProfile;
using VFoody.Application.UseCases.Accounts.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Accounts.Commands.UpdateInfo.UploadAvatar;

public class UpdateLoadAvatarHandler : ICommandHandler<UpdateLoadAvatarCommand, Result>
{
    private readonly IStorageService _storageService;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateProfileHandler> _logger;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IMapper _mapper;

    public UpdateLoadAvatarHandler(IStorageService storageService, IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<UpdateProfileHandler> logger, ICurrentPrincipalService currentPrincipalService, IMapper mapper)
    {
        _storageService = storageService;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentPrincipalService = currentPrincipalService;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(UpdateLoadAvatarCommand request, CancellationToken cancellationToken)
    {
        if (request.Id != this._currentPrincipalService.CurrentPrincipalId)
        {
            throw new AuthenticationException("Id của account và token không map");
        } 

        var account = this._accountRepository.GetAccountWithBuildingByEmail(this._currentPrincipalService.CurrentPrincipal);
        if (account.AvatarUrl != null && account.AvatarUrl.Trim().Length > 0)
        {
            var fileNameImage = account.AvatarUrl.Split("/");
            var isDelete = this._storageService.DeleteFileAsync(fileNameImage[fileNameImage.Length-1]);
        }

        var imagePath = await this._storageService.UploadFileAsync(request.AvatarImageFile);
        await this._unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            account.AvatarUrl = imagePath;
            this._accountRepository.Update(account);
            await this._unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
            return Result.Success(this._mapper.Map<AccountResponse>(account));
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            this._unitOfWork.RollbackTransaction();
            throw new Exception(e.Message);
        }
    }
}