using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class OrderHistoryRepository : BaseRepository<OrderHistory>, IOrderHistoryRepository
{
    public OrderHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}