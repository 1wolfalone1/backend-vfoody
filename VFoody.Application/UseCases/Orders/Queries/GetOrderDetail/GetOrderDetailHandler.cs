using System.Reflection.Metadata;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services.Dapper;
using VFoody.Application.UseCases.Orders.Models;
using VFoody.Application.UseCases.Shop.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Orders.Queries.GetOrderDetail;

public class GetOrderDetailHandler : IQueryHandler<GetOrderDetailQuery, Result>
{
    private readonly IDapperService _dapperService;
    private readonly ILogger<GetOrderDetailHandler> _logger;
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;

    public GetOrderDetailHandler(IDapperService dapperService, ILogger<GetOrderDetailHandler> logger, IShopRepository shopRepository, IMapper mapper)
    {
        _dapperService = dapperService;
        _logger = logger;
        _shopRepository = shopRepository;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
    {
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
            Dictionary<int, ProductInOrderInfoResponse> dicPro = new Dictionary<int, ProductInOrderInfoResponse>();
            Dictionary<int, QuestionInOrderResponse> dic = new Dictionary<int, QuestionInOrderResponse>();
            Func<ProductInOrderInfoResponse, QuestionInOrderResponse, OptionInOrderResponse, ProductInOrderInfoResponse>
                mapProduct =
                    (parent, child1, child2) =>
                    {
                        if (!dic.TryGetValue(child1.QuestionId, out var question))
                        {
                            child1.Options.Add(child2);
                            parent.Topping.Add(child1);
                            dic.Add(child1.QuestionId, child1);
                        }
                        else
                        {
                            parent.Topping.Remove(question);
                            question.Options.Add(child2);
                            parent.Topping.Add(question);
                        }

                        if (!dicPro.TryGetValue(parent.ProductId, out var pro))
                        {
                            dicPro.Add(parent.ProductId, parent);
                        }
                        else
                        {
                            dicPro.Remove(parent.ProductId);
                            dicPro.Add(parent.ProductId, parent);
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
}