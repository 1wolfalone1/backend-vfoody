using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
namespace VFoody.Infrastructure.Persistence.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
