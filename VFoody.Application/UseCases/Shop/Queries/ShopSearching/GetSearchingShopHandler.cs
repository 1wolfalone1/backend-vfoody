using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopSearching;

public class GetSearchingShopHandler : IQueryHandler<GetSearchingShopQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly IFavouriteShopRepository _favouriteShopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly ILogger<GetSearchingShopHandler> _logger;

    public GetSearchingShopHandler(
        IDapperService dapperService, ILogger<GetSearchingShopHandler> logger,
        IFavouriteShopRepository favouriteShopRepository, ICurrentPrincipalService currentPrincipalService
    )
    {
        this._dapperService = dapperService;
        _logger = logger;
        _favouriteShopRepository = favouriteShopRepository;
        _currentPrincipalService = currentPrincipalService;
    }


    public async Task<Result<Result>> Handle(GetSearchingShopQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var accountId = _currentPrincipalService.CurrentPrincipalId!.Value;
            Dictionary<int, SelectDetailsShopDTO> dicUniq = new Dictionary<int, SelectDetailsShopDTO>();
            Func<SelectDetailsShopDTO, SelectSimpleProductOfShopDTO, SelectDetailsShopDTO> map = (parent, child1) =>
            {
                if (!dicUniq.TryGetValue(parent.Id, out var shopInfo))
                {
                    parent.Products.Add(child1);
                    dicUniq.Add(parent.Id, parent);
                    
                }
                else
                {
                    shopInfo.Products.Add(child1);
                    dicUniq.Remove(shopInfo.Id);
                    dicUniq.Add(shopInfo.Id, shopInfo);
                }
                
                return parent;
            };

            await this._dapperService
                .SelectAsync<SelectDetailsShopDTO, SelectSimpleProductOfShopDTO, SelectDetailsShopDTO>(
                    QueryName.SelectSearchShopAndProductInCustomerHome,
                    map,
                    new
                    {
                        SearchText = request.SearchText,
                        CategoryId = request.CategoryId,
                        OrderType = request.OrderType,
                        PageIndex = request.PageIndex,
                        PageSize = request.PageSize,
                        AccountId = accountId,
                        OrderMode = request.OrderMode
                    },
                    "ProductId").ConfigureAwait(false);
            var listResult = dicUniq.Values.ToList();

            var result = new PaginationResponse<SelectDetailsShopDTO>(listResult.ToList(), request.PageIndex,
                request.PageSize, listResult.Count() > 0 ? listResult.First().TotalItems : 0);
            return Result.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}