using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.All;

public class GetAllPromotionForCustomerHandler : IQueryHandler<GetAllPromotionForCustomerQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetAllPromotionForCustomerHandler> _logger;

    public GetAllPromotionForCustomerHandler(IDapperService dapperService, ILogger<GetAllPromotionForCustomerHandler> logger)
    {
        _dapperService = dapperService;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetAllPromotionForCustomerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var listPromotionActive = await _dapperService.SelectAsync<AllPromotionResponse>(QueryName.SelectAllPromotionForOrderCard, new
            {
                ShopId = request.ShopId,
                CustomerId = request.CustomerId,
                Distance = request.Distance,
                OrderValue = request.OrderValue,
                CurrentDate = "2024-06-01",
                Mode = 1
            }).ConfigureAwait(false);
            
            var listPromotionInActive = await _dapperService.SelectAsync<AllPromotionResponse>(QueryName.SelectAllPromotionForOrderCard, new
            {
                ShopId = request.ShopId,
                CustomerId = request.CustomerId,
                Distance = request.Distance,
                OrderValue = request.OrderValue,
                CurrentDate = "2024-06-01",
                Mode = 2
            }).ConfigureAwait(false);
            
            var resultActive = new PaginationResponse<AllPromotionResponse>(listPromotionActive.ToList(), request.PageIndex, request.PageSize, listPromotionActive.First().TotalItems);
            var resultInActive = new PaginationResponse<AllPromotionResponse>(listPromotionInActive.ToList(), request.PageIndex, request.PageSize, listPromotionInActive.First().TotalItems);

            return Result.Success(new
            {
                PromotionActive = resultActive,
                PromotionInActive = resultInActive
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Result.Failure(new Error("500", "Internal server error: " + e.Message));
        }
    }
}