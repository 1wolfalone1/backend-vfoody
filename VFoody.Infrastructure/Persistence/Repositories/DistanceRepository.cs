using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class DistanceRepository : BaseRepository<Distance>, IDistanceRepository
{
    public DistanceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
