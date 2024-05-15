using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class OrderDetailOptionRepository : BaseRepository<OrderDetailOption>, IOrderDetailOptionRepository
{
    public OrderDetailOptionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
