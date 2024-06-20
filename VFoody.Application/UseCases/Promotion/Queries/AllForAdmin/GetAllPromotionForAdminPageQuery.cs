using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Enums;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Promotion.Queries.AllForAdmin;

public class GetAllPromotionForAdminPageQuery : PaginationRequest, IQuery<Result>
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; } = DateTime.Now;
    public PromotionStatus Status { get; set; }
    public PromotionApplyTypes ApplyType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}