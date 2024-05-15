using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class CommissionConfigRepository : BaseRepository<CommissionConfig>, ICommissionConfigRepository
{
    public CommissionConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
