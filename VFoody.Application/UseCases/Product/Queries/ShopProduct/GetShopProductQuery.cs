using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.ShopProduct;

public sealed record GetShopProductQuery(int shopId, int pageNum, int pageSize) : IQuery<Result>;