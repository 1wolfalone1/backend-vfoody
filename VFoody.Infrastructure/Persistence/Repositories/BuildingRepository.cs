using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class BuildingRepository : BaseRepository<Building>, IBuildingRepository
{
    public BuildingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
