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
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.Shop;

public class GetShopPromotionListHandler : IQueryHandler<GetShopPromotionListQuery, Result>
{
    private readonly IDapperService dapperService;
    private readonly ILogger<GetShopPromotionListHandler> _logger;

    public GetShopPromotionListHandler(IDapperService dapperService, ILogger<GetShopPromotionListHandler> logger)
    {
        this.dapperService = dapperService;
        _logger = logger;
    }


    public async Task<Result<Result>> Handle(GetShopPromotionListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await dapperService.SelectAsync<SelectShopPromotionDTO>(QueryName.SelectShopPromotions, new
            {
                request.ShopId,
                request.Status,
                request.StartDate,
                request.EndDate,
                request.Available,
                request.PageIndex,
                request.PageSize,
            }).ConfigureAwait(false);

            var result = new PaginationResponse<SelectShopPromotionDTO>(list.ToList(), request.PageIndex, request.PageSize, list.First().TotalPages);

            return Result.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}
