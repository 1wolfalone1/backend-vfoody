using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Promotion.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.AllPromotionOfShopOwner;

public class GetAllPromotionShopHandler : IQueryHandler<GetAllPromotionShopQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetAllPromotionShopHandler> _logger;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;

    public GetAllPromotionShopHandler(
        IDapperService dapperService, ILogger<GetAllPromotionShopHandler> logger,
        ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository
    )
    {
        _dapperService = dapperService;
        _logger = logger;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(GetAllPromotionShopQuery request, CancellationToken cancellationToken)
    {
        var accountId = _currentPrincipalService.CurrentPrincipalId;
        var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
        try
        {
            var parameter = new
            {
                ActiveStatus = (int)PromotionStatus.Active,
                InActiveStatus = (int)PromotionStatus.UnActive,
                DeleteStatus = (int)PromotionStatus.Delete,
                IsAvailable = request.IsAvailable,
                ShopId = shop.Id,
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