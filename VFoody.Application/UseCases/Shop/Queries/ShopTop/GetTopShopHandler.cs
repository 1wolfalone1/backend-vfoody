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
using VFoody.Application.UseCases.Product.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Application.UseCases.Shop.Queries.ShopSearching;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopTop;

public class GetTopShopHandler : IQueryHandler<GetTopShopQuery, Result>
{
    private readonly IDapperService dapperService;
    private readonly ILogger<GetSearchingShopHandler> _logger;

    public GetTopShopHandler(IDapperService dapperService, ILogger<GetSearchingShopHandler> logger)
    {
        this.dapperService = dapperService;
        _logger = logger;
    }


    public async Task<Result<Result>> Handle(GetTopShopQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await dapperService.SelectAsync<SelectSimpleShopDTO>(QueryName.SelectTopRatingShop, new
            {
                request.PageIndex,
                request.PageSize
            }).ConfigureAwait(false);

            var result = new PaginationResponse<SelectSimpleShopDTO>(list.ToList(), request.PageIndex, request.PageSize, list.First().TotalItems);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}
