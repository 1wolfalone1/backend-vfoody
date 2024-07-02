using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ListShop;

public class GetListShopHandler : IQueryHandler<GetListShopQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly IFavouriteShopRepository _favouriteShopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly ILogger<GetListShopHandler> _logger;


    public GetListShopHandler(IDapperService dapperService, ILogger<GetListShopHandler> logger,
        IFavouriteShopRepository favouriteShopRepository, ICurrentPrincipalService currentPrincipalService
    )
    {
        _dapperService = dapperService;
        _logger = logger;
        _favouriteShopRepository = favouriteShopRepository;
        _currentPrincipalService = currentPrincipalService;
    }

    public async Task<Result<Result>> Handle(GetListShopQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var accountId = _currentPrincipalService.CurrentPrincipalId!.Value;
            var list = await _dapperService.SelectAsync<SelectSimpleShopDTO>(QueryName.SelectShopByIds, new
            {
                ShopIds = request.shopIds
            }).ConfigureAwait(false);

            foreach (var item in list)
            {
                item.IsFavouriteShop = _favouriteShopRepository.IsFavouriteShop(item.Id, accountId);
            }
            if (list != null && list.Count() > 0)
            {
                return Result.Success(list);
            }

            return Result.Failure(new Error("404",
                $"Không tìm thấy danh sách shop với ids: {string.Join(", ", request.shopIds)}"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}