using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Feedbacks.Queries.ShopFeedbacks;

public class GetAllShopFeedbackQuery : PaginationRequest,IQuery<Result>
{
    public int Id { get; set; }
}