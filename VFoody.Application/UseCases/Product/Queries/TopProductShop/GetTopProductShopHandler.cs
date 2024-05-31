using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Responses;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.TopProductShop;

public class GetTopProductShopHandler : IQueryHandler<GetTopProductShopQuery, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetTopProductShopHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetTopProductShopQuery request, CancellationToken cancellationToken)
    {
        var products = _productRepository.GetTopProductByShopId(request.ShopId, request.PageIndex, request.PageSize);
        var totalProducts = _productRepository.CountTotalActiveProductByShopId(request.ShopId);
        var result = new PaginationResponse<ProductResponse>(_mapper.Map<List<ProductResponse>>(products),
            request.PageIndex, request.PageSize, totalProducts);
        return Result.Success(result);
    }
}