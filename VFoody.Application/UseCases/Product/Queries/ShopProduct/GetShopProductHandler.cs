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

    public async Task<Result<Result>> Handle(GetShopProductQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.DbSet
            .Where(p => p.ShopId == request.shopId && p.Status == (int)ProductStatus.Active)
            .Skip((request.pageNum - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToListAsync(cancellationToken: cancellationToken);
        return Result.Success(_mapper.Map<List<ProductResponse>>(products));
    }
}