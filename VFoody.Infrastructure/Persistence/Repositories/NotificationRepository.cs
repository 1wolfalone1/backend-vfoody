using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    public NotificationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public (List<Notification> notifications, int totalItems) GetCustomerNotifications(int requestPageIndex,
        int requestPageSize,
        int currentPrincipalId)
    {
        var query = this.DbSet.AsQueryable();
        query = query.Where(n => n.AccountId == currentPrincipalId &&
                                 n.RoleId == (int) Roles.Customer);
        int totalItems = query.Count();
        var notifications = query
            .Skip((requestPageIndex - 1) * requestPageSize)
            .Take(requestPageSize).ToList();

        return (notifications, totalItems);
    }
}
