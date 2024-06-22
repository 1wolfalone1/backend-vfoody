using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.ProductDetailShopOwner;

public sealed record GetProductDetailQuery(int productId, int? shopId) : IQuery<Result>;