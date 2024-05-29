using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopInfo;

public class GetShopInfoHandler: IQueryHandler<GetShopInfoQuery, Result>
{
    private readonly IShopRepository _shopRepository;
    private readonly ILogger<GetShopInfoHandler> _logger;

    public GetShopInfoHandler(IShopRepository shopRepository, ILogger<GetShopInfoHandler> logger)
    {
        _shopRepository = shopRepository;
        _logger = logger;
    }

    public Task<Result<Result>> Handle(GetShopInfoQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}