using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.ShopProduct;

public class GetShopProductHandler : IQueryHandler<GetShopProductQuery, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetShopProductHandler> _logger;

    public GetShopProductHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetShopProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public Task<Result<Result>> Handle(GetShopProductQuery request, CancellationToken cancellationToken)
    {
        var products = _productRepository.GetShopProduct(request.shopId, request.pageNum, request.pageSize);
        return Task.FromResult<Result<Result>>(Result.Success(_mapper.Map<List<ProductResponse>>(products)));
    }
}