using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class ContributionTypeRepository : BaseRepository<ContributionType>, IContributionTypeRepository
{
    public ContributionTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
