using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.ShopProduct;

public class GetShopProductHandler : IQueryHandler<GetShopProductQuery, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetShopProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetShopProductQuery request, CancellationToken cancellationToken)
    {
        var totalProducts = _productRepository.CountTotalActiveProductByShopId(request.ShopId);
        var products = _productRepository.GetShopProduct(request.ShopId, request.PageIndex, request.PageSize);
        var result = new PaginationResponse<ProductResponse>(_mapper.Map<List<ProductResponse>>(products),
            request.PageIndex, request.PageSize, totalProducts);
        return Result.Success(result);
    }
}