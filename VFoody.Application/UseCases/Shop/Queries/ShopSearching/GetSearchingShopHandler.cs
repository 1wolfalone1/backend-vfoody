using ArtHubBO.Payload;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Accounts.Commands;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopSearching;

public class GetSearchingShopHandler : IQueryHandler<GetSearchingShopQuery, Result>
{
    private readonly IDapperService dapperService;
    private readonly ILogger<GetSearchingShopHandler> _logger;

    public GetSearchingShopHandler(IDapperService dapperService, ILogger<GetSearchingShopHandler> logger)
    {
        this.dapperService = dapperService;
        _logger = logger;
    }


    public async Task<Result<Result>> Handle(GetSearchingShopQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await dapperService.SelectAsync<SelectDetailsShopDTO>(QueryName.SelectSearchingShop, new
            {
                request.PageIndex,
                request.PageSize,
                request.SearchText,
                request.OrderType,
                request.CurrentBuildingId,
            }).ConfigureAwait(false);

            var MAX_GET_PRODUCT_LIST_SIZE = 32;
            foreach (var shop in list)
            {
                var products = await dapperService.SelectAsync<SelectSimpleProductOfShopDTO>(QueryName.SelectAvailableProductListOfShop, new
                {
                    request.PageIndex,
                    PageSize = MAX_GET_PRODUCT_LIST_SIZE,
                    ShopId = shop.Id,
                    request.SearchText,
                }).ConfigureAwait(false);

                shop.Products = products.ToList();
            }
            var result = new PaginationResponse<SelectDetailsShopDTO>(list.ToList(), request.PageIndex, request.PageSize, list.First().TotalPages);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}
