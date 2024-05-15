using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;
public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
