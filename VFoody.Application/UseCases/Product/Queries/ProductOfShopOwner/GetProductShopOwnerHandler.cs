using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.ProductOfShopOwner;

public class GetProductShopOwnerHandler : IQueryHandler<GetProductShopOwnerQuery, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IShopRepository _shopRepository;
    private readonly IMapper _mapper;

    public GetProductShopOwnerHandler(
        IProductRepository productRepository, IMapper mapper,
        ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository
    )
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(GetProductShopOwnerQuery request, CancellationToken cancellationToken)
    {
        int shopId;
        if (request.ShopId != null)
        {
            shopId = request.ShopId.Value;
        }
        else
        {
            var accountId = _currentPrincipalService.CurrentPrincipalId;
            var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
            shopId = shop.Id;
        }

        var totalProduct = _productRepository.CountTotalProductByShopId(shopId);
        var productList =
            await _productRepository.GetListProductByShopId(
                shopId, request.Status, request.PageIndex, request.PageSize);
        var result = new PaginationResponse<ProductShopOwnerResponse>(
            _mapper.Map<List<ProductShopOwnerResponse>>(productList),
            request.PageIndex, request.PageSize, totalProduct);
        return Result.Success(result);
    }
}