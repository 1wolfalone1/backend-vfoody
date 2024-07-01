using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.AllPromotionOfShopOwner;

public class GetAllPromotionShopHandler : IQueryHandler<GetAllPromotionShopQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetAllPromotionShopHandler> _logger;
    private readonly IMapper _mapper;

    public GetAllPromotionShopHandler(
        IDapperService dapperService, ILogger<GetAllPromotionShopHandler> logger, IMapper mapper
    )
    {
        _dapperService = dapperService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetAllPromotionShopQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var parameter = new
            {
                DeleteStatus = (int)PromotionStatus.Delete,
                ShopId = 1,
                SearchValue = request.SearchValue,
                FilterByTime = request.FilterByTime,
                DateFrom = request.DateFrom?.ToString("yyyy-MM-dd"),
                DateTo = request.DateTo?.ToString("yyyy-MM-dd"),
                OrderBy = request.OrderBy,
                Direction = request.Direction,
                Offset = (request.PageIndex - 1) * request.PageSize,
                PageSize = request.PageSize
            };

            var totalShop = await _dapperService.SingleOrDefaultAsync<int>(
                    QueryName.CountAllPromotionOfShopByCondition, parameter)
                .ConfigureAwait(false);


            var shops = await _dapperService.SelectAsync<ShopPromotionResponse>(
                    QueryName.SelectAllPromotionOfShopByCondition, parameter)
                .ConfigureAwait(false);

            var result = new PaginationResponse<ShopPromotionResponse>(
                shops.ToList(), request.PageIndex, request.PageSize, totalShop
            );
            return Result.Success(result);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal error"));
        }
    }
}