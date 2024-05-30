using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.TopProductShop;

public sealed record GetTopProductShopQuery(int shopId, int pageNum, int pageSize) : IQuery<Result>;