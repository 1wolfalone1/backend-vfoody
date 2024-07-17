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
using VFoody.Application.UseCases.Shop.Queries.ShopSearching;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries;

public class GetRecentOrderedProductHandler : IQueryHandler<GetRecentOrderedProductQuery, Result>
{
    private readonly IDapperService dapperService;
    private readonly ILogger<GetSearchingShopHandler> _logger;

    public GetRecentOrderedProductHandler(IDapperService dapperService, ILogger<GetSearchingShopHandler> logger)
    {
        this.dapperService = dapperService;
        this._logger = logger;
    }


    public async Task<Result<Result>> Handle(GetRecentOrderedProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await this.dapperService.SelectAsync<SelectSimpleProductDTO>(QueryName.SelectRecentOrderedProduct, new
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Email = request.Email
            }).ConfigureAwait(false);

            var result = new PaginationResponse<SelectSimpleProductDTO>(list.ToList(), request.PageIndex, request.PageSize, list.ToList().Count > 0 ? list.First().TotalItems : 0);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}
