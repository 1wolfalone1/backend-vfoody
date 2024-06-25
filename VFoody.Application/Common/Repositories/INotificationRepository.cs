using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    (List<Notification> notifications, int totalItems) GetCustomerNotifications(int requestPageIndex,
        int requestPageSize, int currentPrincipalId);
}
