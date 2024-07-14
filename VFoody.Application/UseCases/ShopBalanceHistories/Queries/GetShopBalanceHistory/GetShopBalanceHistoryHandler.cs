using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.ShopBalanceHistories.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopBalanceHistories.Queries.GetShopBalanceHistory;

public class GetShopBalanceHistoryHandler : IQueryHandler<GetShopBalanceHistoryQuery, Result>
{
    private readonly IShopBalanceHistoryRepository _balanceHistoryRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;

    public GetShopBalanceHistoryHandler(IShopBalanceHistoryRepository balanceHistoryRepository, IMapper mapper, ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository)
    {
        _balanceHistoryRepository = balanceHistoryRepository;
        _mapper = mapper;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(GetShopBalanceHistoryQuery request, CancellationToken cancellationToken)
    {
        var shop = await this._shopRepository.GetShopByAccountId(this._currentPrincipalService.CurrentPrincipalId
            .Value);
        var shopBalance =
            this._balanceHistoryRepository.GetShopBalanceHistory(request.PageIndex, request.PageSize, shop.Id, request.TransactionType);
        PaginationResponse<ShopBalanceHistoryResponse> result = new PaginationResponse<ShopBalanceHistoryResponse>();
        result.PageIndex = request.PageIndex;
        result.PageSize = request.PageSize;
        var listShopBalanceResponse = this._mapper.Map<List<ShopBalanceHistoryResponse>>(shopBalance.ListShopBalance);
        result.Items = listShopBalanceResponse;
        result.NumberOfItems = shopBalance.TotalItem;

        return Result.Success(result);
    }
}