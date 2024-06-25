using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Queries.GetShopOrderByStatus;

public class GetShopOrderByStatusHandler : IQueryHandler<GetShopOrderByStatusQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly ILogger<GetShopOrderByStatusHandler> _logger;

    public GetShopOrderByStatusHandler(IDapperService dapperService, ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository, ILogger<GetShopOrderByStatusHandler> logger)
    {
        _dapperService = dapperService;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
        _logger = logger;
    }

    public async Task<Result<Result>> Handle(GetShopOrderByStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var shop = await this._shopRepository.GetShopByAccountId(this._currentPrincipalService.CurrentPrincipalId
                .Value);
            var listOrder = await this._dapperService.SelectAsync<OrderOfShopResponse>(QueryName.SelectOrderForShop,
                new
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Status = request.Status,
                    ShopId = shop.Id
                }).ConfigureAwait(false);

            var result = new PaginationResponse<OrderOfShopResponse>(listOrder.ToList(), request.PageIndex,
                request.PageSize,
                listOrder.Count() > 0 ? listOrder.First().TotalItems : 0);
            return Result.Success(result);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            throw;
        }
    }
}