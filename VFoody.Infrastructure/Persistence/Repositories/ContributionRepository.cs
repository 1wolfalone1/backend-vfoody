using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class ContributionRepository : BaseRepository<Contribution>, IContributionRepository
{
    public ContributionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
