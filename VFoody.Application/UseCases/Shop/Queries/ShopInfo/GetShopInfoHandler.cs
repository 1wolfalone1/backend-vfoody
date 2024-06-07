using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopInfo;

public class GetShopInfoHandler : IQueryHandler<GetShopInfoQuery, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;

    public GetShopInfoHandler(IShopRepository shopRepository, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _mapper = mapper;
    }

    public Task<Result<Result>> Handle(GetShopInfoQuery request, CancellationToken cancellationToken)
    {
        var shop = _shopRepository.GetInfoByShopIdAndStatusIn(request.shopId, new int[]{(int)ShopStatus.Active});
        return Task.FromResult<Result<Result>>(shop != null
            ? Result.Success(_mapper.Map<ShopInfoResponse>(shop))
            : Result.Failure(new Error("400", "Not found this shop.")));
    }
}