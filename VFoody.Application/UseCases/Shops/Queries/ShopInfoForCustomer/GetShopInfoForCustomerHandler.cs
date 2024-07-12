using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopInfo;

public class GetShopInfoForCustomerHandler : IQueryHandler<GetShopInfoForCustomerQuery, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IFavouriteShopRepository _favouriteShopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IMapper _mapper;

    public GetShopInfoForCustomerHandler(IShopRepository shopRepository, IMapper mapper,
        IFavouriteShopRepository favouriteShopRepository, ICurrentPrincipalService currentPrincipalService, IAccountRepository accountRepository)
    {
        _shopRepository = shopRepository;
        _mapper = mapper;
        _favouriteShopRepository = favouriteShopRepository;
        _currentPrincipalService = currentPrincipalService;
        _accountRepository = accountRepository;
    }

    public Task<Result<Result>> Handle(GetShopInfoForCustomerQuery request, CancellationToken cancellationToken)
    {
        var shop = _shopRepository.GetInfoByShopIdAndStatusIn(request.shopId, new int[] { (int)ShopStatus.Active });
        var accountId = _currentPrincipalService.CurrentPrincipalId!.Value;
        var account = this._accountRepository.GetById(shop.AccountId);
        var isFavouriteShop = _favouriteShopRepository.IsFavouriteShop(request.shopId, accountId);
        var shopInfoResponse = _mapper.Map<ShopInfoResponse>(shop);
        shopInfoResponse.IsFavouriteShop = isFavouriteShop;
        shopInfoResponse.Email = account.Email;
        return Task.FromResult<Result<Result>>(shop != null
            ? Result.Success(shopInfoResponse)
            : Result.Failure(new Error("400", "Not found this shop.")));
    }
}