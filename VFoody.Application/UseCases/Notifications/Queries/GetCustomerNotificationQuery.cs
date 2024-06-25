using VFoody.Application.Common.Abstractions.Messaging;
using VFoody.Application.Common.Models.Requests;
using VFoody.Domain.Shared;

namespace VFoody.Application.UseCases.Notifications.Queries;

public class GetCustomerNotificationQuery : PaginationRequest, IQuery<Result>
{
}