

using AutoMapper;
using Microsoft.Extensions.Logging;
using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Repositories;
using VFoody.Application.UseCases.Product.Models;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.CardProducts;

public class GetListProductInCardHandler : IQueryHandler<GetListProductInCardQuery, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public GetListProductInCardHandler(IProductRepository productRepository, ILogger<GetListProductInCardHandler> logger, IMapper mapper)
    {
        _productRepository = productRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<Result>> Handle(GetListProductInCardQuery request, CancellationToken cancellationToken)
    {
        var products = await this._productRepository.GetListProductInCard(request.ProductIds).ConfigureAwait(false);

        if (products != null && products.Count > 0)
        {
            var result = products.Select(x => this._mapper.Map<ProductCardResponse>(x)).ToList();
            return Result.Success(result);
        }

        return Result.Failure(new Error("404", $"Không tìm thấy danh sách sản phẩm {string.Join(", ", request.ProductIds)}"));
    }
}