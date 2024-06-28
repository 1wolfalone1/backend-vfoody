using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Notifications.Queries.ShopOwnerNotification;

public class GetShopOwnerNotificationQuery : PaginationRequest, IQuery<Result>
{

}