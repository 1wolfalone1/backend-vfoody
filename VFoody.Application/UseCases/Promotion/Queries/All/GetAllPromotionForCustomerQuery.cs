using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.All;

public class GetAllPromotionForCustomerQuery : PaginationRequest, IQuery<Result>
{
    public int ShopId { get; set; }
    public int CustomerId { get; set; }
    public double Distance { get; set; }
    public double OrderValue { get; set; }
}