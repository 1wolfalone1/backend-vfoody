using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.AllPromotionOfShopOwner;

public class GetAllPromotionShopQuery : PaginationRequest, IQuery<Result>
{
    public string? SearchValue { get; set; }
    public int? FilterByTime { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? OrderBy { get; set; }
    public int? Direction { get; set; }
}