using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopInfoForShop;

public class GetShopProfileHandler : IQueryHandler<GetShopProfileQuery, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IMapper _mapper;

    public GetShopProfileHandler(IShopRepository shopRepository, ICurrentPrincipalService currentPrincipalService, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _currentPrincipalService = currentPrincipalService;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetShopProfileQuery request, CancellationToken cancellationToken)
    {
        var shopInfo =
            await this._shopRepository.GetShopProfileByAccountIdAsync(this._currentPrincipalService.CurrentPrincipalId.Value)
                .ConfigureAwait(false);
        var shopResponse = this._mapper.Map<Models.ShopProfileResponse>(shopInfo);
        return Result.Success(shopResponse);
    }
}