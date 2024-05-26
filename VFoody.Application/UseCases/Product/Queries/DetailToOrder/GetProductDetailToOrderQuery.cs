using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.DetailToOrder;

public sealed record GetProductDetailToOrderQuery(int productId) : IQuery<Result>;