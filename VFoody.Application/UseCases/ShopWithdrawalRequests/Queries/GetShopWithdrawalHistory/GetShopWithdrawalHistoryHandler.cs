using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.ShopWithdrawalRequests.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.ShopWithdrawalRequests.Queries.GetShopWithdrawalHistory;

public class GetShopWithdrawalHistoryHandler : IQueryHandler<GetShopWithdrawalHistoryQuery, Result>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IShopWithdrawalRequestRepository _withdrawalRequestRepository;
    private readonly IShopRepository _shopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IMapper _mapper;

    public GetShopWithdrawalHistoryHandler(IAccountRepository accountRepository, IShopWithdrawalRequestRepository withdrawalRequestRepository, IShopRepository shopRepository, ICurrentPrincipalService currentPrincipalService, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _withdrawalRequestRepository = withdrawalRequestRepository;
        _shopRepository = shopRepository;
        _currentPrincipalService = currentPrincipalService;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetShopWithdrawalHistoryQuery request, CancellationToken cancellationToken)
    {
        var shop = await this._shopRepository.GetShopByAccountId(this._currentPrincipalService.CurrentPrincipalId.Value);
        if (shop == null)
            throw new InvalidBusinessException("Không tìm thấy cửa hàng của bạn");

        var shopWithdrawal =
            this._withdrawalRequestRepository.GetShopWithdrawalHistory(request.PageSize, request.PageIndex, shop.Id, (int)request.Status);

        PaginationResponse<ShopWithdrawalHistoryResponse> result =
            new PaginationResponse<ShopWithdrawalHistoryResponse>();
        result.PageIndex = request.PageIndex;
        result.PageSize = request.PageSize;
        var listResponse = this._mapper.Map<List<ShopWithdrawalHistoryResponse>>(shopWithdrawal.ListWithdrawals);
        result.Items = listResponse;
        result.NumberOfItems = shopWithdrawal.TotalItem;
        return Result.Success(result);
    }
}