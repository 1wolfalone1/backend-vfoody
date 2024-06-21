using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.FavouriteShop.Commands.FavouriteShop;

public class FavouriteShopHandler : ICommandHandler<FavouriteShopCommand, Result>
{
    private readonly IFavouriteShopRepository _favouriteShopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FavouriteShopHandler> _logger;

    public FavouriteShopHandler(
        IFavouriteShopRepository favouriteShopRepository, ICurrentPrincipalService currentPrincipalService,
        IUnitOfWork unitOfWork, ILogger<FavouriteShopHandler> logger, IShopRepository shopRepository)
    {
        _favouriteShopRepository = favouriteShopRepository;
        _currentPrincipalService = currentPrincipalService;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(FavouriteShopCommand request, CancellationToken cancellationToken)
    {
        //Check shop active
        var isActiveShop = _shopRepository.IsActiveShop(request.ShopId);
        if (!isActiveShop)
        {
            return Result.Failure(new Error("400", "Shop không tồn tại."));
        }
        var accountId = _currentPrincipalService.CurrentPrincipalId!.Value;
        var favouriteShop = _favouriteShopRepository.GetByShopIdAndAccountId(request.ShopId, accountId);

        try
        {
            //Begin transaction
            await _unitOfWork.BeginTransactionAsync();

            if (favouriteShop != null)
            {
                //Remove this favourite shop for this account id
                _favouriteShopRepository.Remove(favouriteShop);
            }
            else
            {
                //Create new favourite shop for this account id
                var newFavouriteShop = new Domain.Entities.FavouriteShop()
                {
                    ShopId = request.ShopId,
                    AccountId = accountId
                };
                await _favouriteShopRepository.AddAsync(newFavouriteShop);
            }

            await _unitOfWork.CommitTransactionAsync();
            return Result.Create("Update favourite shop successfully.");
        }
        catch (Exception e)
        {
            //Rollback when exception
            _unitOfWork.RollbackTransaction();
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error."));
        }
    }
}