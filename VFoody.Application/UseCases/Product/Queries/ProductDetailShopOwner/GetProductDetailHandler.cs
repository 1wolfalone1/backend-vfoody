using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.Common.Services;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.ProductDetailShopOwner;

public class GetProductDetailHandler : IQueryHandler<GetProductDetailQuery, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IShopRepository _shopRepository;
    private readonly ICurrentPrincipalService _currentPrincipalService;
    private readonly IMapper _mapper;

    public GetProductDetailHandler(IProductRepository productRepository, IMapper mapper,
        ICurrentPrincipalService currentPrincipalService, IShopRepository shopRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _currentPrincipalService = currentPrincipalService;
        _shopRepository = shopRepository;
    }

    public async Task<Result<Result>> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
    {
        int shopId;
        if (request.shopId != null)
        {
            shopId = request.shopId.Value;
        }
        else
        {
            var accountId = _currentPrincipalService.CurrentPrincipalId;
            var shop = await _shopRepository.GetShopByAccountId(accountId!.Value);
            shopId = shop.Id;
        }
        var product = _productRepository.GetProductDetailShopOwner(request.productId, shopId);
        return await Task.FromResult<Result<Result>>(product != null
            ? Result.Success(_mapper.Map<ProductDetailResponse>(product))
            : Result.Failure(new Error("400", "Not found this product.")));
    }
}