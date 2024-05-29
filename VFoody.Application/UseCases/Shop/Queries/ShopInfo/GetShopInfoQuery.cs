using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Shop.Queries.ShopInfo;

public sealed record GetShopInfoQuery(int shopId) : IQuery<Result>;