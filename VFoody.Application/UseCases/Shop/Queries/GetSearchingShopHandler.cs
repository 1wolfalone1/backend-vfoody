using ArtHubBO.Payload;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries;

public class GetSearchingShopHandler : IQueryHandler<GetSearchingShopQuery, Result>
{
    private readonly IDapperService dapperService;
    private readonly IAccountService accountService;
    private readonly ITestService testService;

    public GetSearchingShopHandler(IDapperService dapperService, ITestService testService)
    {
        this.dapperService = dapperService;
        this.testService = testService;
    }


    public async Task<Result<Result>> Handle(GetSearchingShopQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await this.dapperService.SelectAsync<SelectDetailsShopDTO>(QueryName.SelectSearchingShop, new
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SearchText = request.SearchText,
                OrderType = request.OrderType,
                CurrentBuildingId = request.CurrentBuildingId,
            }).ConfigureAwait(false);

            var MAX_GET_PRODUCT_LIST_SIZE = 32;
            foreach(var shop in list)
            {
                var products = await this.dapperService.SelectAsync<SelectSimpleProductOfShopDTO>(QueryName.SelectAvailableProductListOfShop, new
                {
                    PageIndex = request.PageIndex,
                    PageSize = MAX_GET_PRODUCT_LIST_SIZE,
                    ShopId = shop.Id,
                    SearchText = request.SearchText,
                }).ConfigureAwait(false);

                shop.Products = products.ToList();
            }
            var result = new PaginationResponse<SelectDetailsShopDTO>(list.ToList(), request.PageIndex, request.PageSize, list.First().TotalPages);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            // logging exception
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}
