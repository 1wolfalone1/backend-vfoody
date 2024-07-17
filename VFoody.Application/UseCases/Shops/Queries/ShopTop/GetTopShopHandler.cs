using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Application.UseCases.Shop.Queries.ShopSearching;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopTop;

public class GetTopShopHandler : IQueryHandler<GetTopShopQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly IFavouriteShopRepository _favouriteShopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly ILogger<GetSearchingShopHandler> _logger;

    public GetTopShopHandler(
        IDapperService dapperService, ILogger<GetSearchingShopHandler> logger,
        IFavouriteShopRepository favouriteShopRepository, ICurrentPrincipalService currentPrincipalService
    )
    {
        this._dapperService = dapperService;
        _logger = logger;
        _favouriteShopRepository = favouriteShopRepository;
        _currentPrincipalService = currentPrincipalService;
    }


    public async Task<Result<Result>> Handle(GetTopShopQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var accountId = _currentPrincipalService.CurrentPrincipalId!.Value;
            var list = await _dapperService.SelectAsync<SelectSimpleShopDTO>(QueryName.SelectTopRatingShop, new
            {
                request.PageIndex,
                request.PageSize
            }).ConfigureAwait(false);
            var response = list.ToList();
            response.ForEach(s => s.IsFavouriteShop = _favouriteShopRepository.IsFavouriteShop(s.Id, accountId));
            var result = new PaginationResponse<SelectSimpleShopDTO>(response, request.PageIndex, request.PageSize,
                list.ToList().Count > 0 ? list.First().TotalItems : 0);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}