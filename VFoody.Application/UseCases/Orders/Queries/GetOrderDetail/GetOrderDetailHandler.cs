using System.Reflection.Metadata;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Constants;
using VFoody.Application.Common.Exceptions;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Queries.GetOrderDetail;

public class GetOrderDetailHandler : IQueryHandler<GetOrderDetailQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetOrderDetailHandler> _logger;
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentAccountService _currentAccountService;
    
    public GetOrderDetailHandler(IDapperService dapperService, ILogger<GetOrderDetailHandler> logger, IShopRepository shopRepository, IMapper mapper, ICurrentPrincipalService currentPrincipalService, IOrderRepository orderRepository, ICurrentAccountService currentAccountService)
    {
        _dapperService = dapperService;
        _logger = logger;
        _shopRepository = shopRepository;
        _mapper = mapper;
        _currentPrincipalService = currentPrincipalService;
        _orderRepository = orderRepository;
        _currentAccountService = currentAccountService;
    }

    public async Task<Result<Result>> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
    {
        // Validate
        await this.ValidateGetOrderDetail(request.OrderId);
        try
        {
            var result = new OrderDetailResponse();
            // Map Order Infor
            Func<OrderInfoResponse, BuildingResponse, PromotionInOrderInfoResponse, OrderInfoResponse> map =
                (parent, child1, child2) =>
                {
                    if (child1 != null)
                    {
                        parent.Building = child1;
                    }

                    if (parent != null)
                    {
                        parent.Voucher = child2;
                    }

                    return parent;
                };

            var listOrderInfo = await _dapperService.SelectAsync(QueryName.SelectOrderAndVoucherInfor,
                map,
                new
                {
                    request.OrderId
                },
                "BuildingId,PromotionId");
            result.OrderInfo = listOrderInfo.SingleOrDefault();
            var shop = _shopRepository.GetInfoByShopIdAndStatusIn(result.OrderInfo.ShopId, new int[]{(int)ShopStatus.Active});
            result.ShopInfo = this._mapper.Map<ShopInfoResponse>(shop);
            
            // Map Product Infor
            Dictionary<string, ProductInOrderInfoResponse> dicPro = new Dictionary<string, ProductInOrderInfoResponse>();
            Dictionary<string, QuestionInOrderResponse> dic = new Dictionary<string, QuestionInOrderResponse>();
            Func<ProductInOrderInfoResponse, QuestionInOrderResponse, OptionInOrderResponse, ProductInOrderInfoResponse>
                mapProduct =
                    (parent, child1, child2) =>
                    {
                        var parentId = parent.ProductId + StringPatterConstants.SEPARATE_ORDERID + parent.OrderDetailId;
                        var questionId = string.Empty;
                        if (child1 != null)
                        {
                            questionId = parent.ProductId + StringPatterConstants.SEPARATE_ORDERID + child1.QuestionId 
                                         + StringPatterConstants.SEPARATE_ORDERID + parent.OrderDetailId;
                        }

                        if (dicPro.TryGetValue(parentId, out var product))
                        {
                            parent = product;
                        }
                        
                        if (child1 != null && !dic.TryGetValue(questionId, out var question))
                        {
                            child1.Options.Add(child2);
                            parent.Topping.Add(child1);
                            dic.Add(questionId, child1);
                        }
                        else if(child1 != null && dic.TryGetValue(questionId, out var que))
                        {
                            parent.Topping.Remove(que);
                            que.Options.Add(child2);
                            parent.Topping.Add(que);
                        }

                        if (!dicPro.TryGetValue(parentId, out var pro))
                        {
                            dicPro.Add(parentId, parent);
                        }
                        else
                        {
                            dicPro.Remove(parentId);
                            dicPro.Add(parentId, parent);
                        }

                        return parent;
                    };

            var listProductInfor = await _dapperService.SelectAsync(QueryName.SelectProductForOrderInfo,
                mapProduct,
                new
                {
                    OrderId = request.OrderId
                }, "QuestionId,OptionId").ConfigureAwait(false);

            result.Products = dicPro.Values.ToList();
            return Result.Success(result);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, e.Message);
            throw;
        }
    }

    private async Task ValidateGetOrderDetail(int orderId)
    {

        var account = this._currentAccountService.GetCurrentAccount();
        if (account.RoleId == (int)Domain.Enums.Roles.Customer)
        {
            var order = await this._orderRepository.Get(or => or.Id == orderId
                                                              && or.AccountId == account.Id).SingleOrDefaultAsync();
            if(order == default)
                throw new InvalidBusinessException($"Bạn không có quyền xem chi tiết đơn hàng này");
        }

        if (account.RoleId == (int)Domain.Enums.Roles.Shop)
        {
            var shop = await this._shopRepository.GetShopByAccountId(this._currentPrincipalService.CurrentPrincipalId.Value);
            var order = await this._orderRepository.GetOrderOfShopByIdAsync(orderId, shop.Id);
            if (order == default)
                throw new InvalidBusinessException($"Cửa hàng bạn không có quyền xem chi tiết đơn hàng này");
        }
    }
}