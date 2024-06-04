using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Product.Queries.CardProducts;

public class GetListProductInCardQuery : IQuery<Result>
{
    public int[] ProductIds { get; set; }
}