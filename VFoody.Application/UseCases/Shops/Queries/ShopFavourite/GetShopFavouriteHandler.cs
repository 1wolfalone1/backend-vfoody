using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopFavourite;

public class GetShopFavouriteHandler : IQueryHandler<GetShopFavouriteQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly IFavouriteShopRepository _favouriteShopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly ILogger<GetShopFavouriteHandler> _logger;

    public GetShopFavouriteHandler(
        IFavouriteShopRepository favouriteShopRepository, ILogger<GetShopFavouriteHandler> logger,
        ICurrentPrincipalService currentPrincipalService, IDapperService dapperService)
    {
        _favouriteShopRepository = favouriteShopRepository;
        _logger = logger;
        _currentPrincipalService = currentPrincipalService;
        _dapperService = dapperService;
    }
    public async Task<Result<Result>> Handle(GetShopFavouriteQuery request, CancellationToken cancellationToken)
    {
        var favouriteShop = await _favouriteShopRepository.GetByAccountId(_currentPrincipalService.CurrentPrincipalId!.Value);
        PaginationResponse<SelectSimpleShopDTO>? result;
        if (favouriteShop.Count == 0)
        {
            result = new PaginationResponse<SelectSimpleShopDTO>(new List<SelectSimpleShopDTO>(), request.PageIndex, request.PageSize,
                0);
            return Result.Success(result);
        }
        try
        {
            var shopIds = favouriteShop.Select(fs => fs.ShopId).ToList();
            var parameter = new
            {
                ActiveStatus = (int)ShopStatus.Active,
                ShopIds = shopIds,
                Offset = (request.PageIndex - 1) * request.PageSize,
                PageSize = request.PageSize
            };

            var response = await this._dapperService.SelectAsync<SelectSimpleShopDTO>(
                    QueryName.SelectFavouriteShop, parameter)
                .ConfigureAwait(false);
            response.ToList().ForEach(r => r.IsFavouriteShop = true);

            result = new PaginationResponse<SelectSimpleShopDTO>(response.ToList(), request.PageIndex, request.PageSize,
                response.First().TotalItems);
            return Result.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}