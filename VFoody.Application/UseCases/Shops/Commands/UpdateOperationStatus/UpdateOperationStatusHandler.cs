using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shops.Commands.UpdateOperationStatus;

public class UpdateOperationStatusHandler : ICommandHandler<UpdateOperationStatusCommand, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly ILogger<UpdateOperationStatusHandler> _logger;

    public UpdateOperationStatusHandler(
        IShopRepository shopRepository, IUnitOfWork unitOfWork,
        ILogger<UpdateOperationStatusHandler> logger, ICurrentPrincipalService currentPrincipalService
    )
    {
        _shopRepository = shopRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currentPrincipalService = currentPrincipalService;
    }

    public async Task<Result<Result>> Handle(UpdateOperationStatusCommand request, CancellationToken cancellationToken)
    {
        var accountId = _currentPrincipalService.CurrentPrincipalId!.Value;
        var shop = await _shopRepository.GetShopByAccountId(accountId);
        try
        {
            //Begin transaction
            await _unitOfWork.BeginTransactionAsync();
            if (shop.Active == 0)
            {
                shop.Active = 1;
            }
            else
            {
                shop.Active = 0;
            }
            _shopRepository.Update(shop);
            //Commit transaction
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            //Rollback when exception
            _unitOfWork.RollbackTransaction();
            _logger.LogError(ex, ex.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }

        return Result.Success(shop.Active == 0 ? "Shop đã ngưng hoạt động" : "Shop đã hoạt động trở lại");
    }
}