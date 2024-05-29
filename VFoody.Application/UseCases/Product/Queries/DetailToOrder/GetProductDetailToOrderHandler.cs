using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.DetailToOrder;

public class GetProductDetailToOrderHandler : IQueryHandler<GetProductDetailToOrderQuery, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductDetailToOrderHandler> _logger;

    public GetProductDetailToOrderHandler(IProductRepository productRepository,
        ILogger<GetProductDetailToOrderHandler> logger, IMapper mapper)
    {
        _productRepository = productRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public Task<Result<Result>> Handle(GetProductDetailToOrderQuery request, CancellationToken cancellationToken)
    {
        var product = _productRepository.GetProductDetail(request.productId);
        return Task.FromResult<Result<Result>>(product != null
            ? Result.Success(_mapper.Map<ProductDetailResponse>(product))
            : Result.Failure(new Error("400", "Not found this product.")));
    }
}