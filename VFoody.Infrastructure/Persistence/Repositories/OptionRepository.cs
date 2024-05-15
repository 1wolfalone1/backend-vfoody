using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class OptionRepository : BaseRepository<Option>, IOptionRepository
{
    public OptionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
