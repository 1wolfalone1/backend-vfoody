using AutoMapper;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.ProductDetailShopOwner;

public class GetProductDetailHandler : IQueryHandler<GetProductDetailQuery, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductDetailHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public Task<Result<Result>> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
    {
        var product = _productRepository.GetProductDetailShopOwner(request.productId);
        return Task.FromResult<Result<Result>>(product != null
            ? Result.Success(_mapper.Map<ProductDetailResponse>(product))
            : Result.Failure(new Error("400", "Not found this product.")));
    }
}