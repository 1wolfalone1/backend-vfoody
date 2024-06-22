using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopDetail;

public sealed record GetShopDetailQuery(int ShopId) : IQuery<Result>;