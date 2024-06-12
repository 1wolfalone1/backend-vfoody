using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<bool> CheckExistCategoryByIds(List<int> ids);
    Task<List<Category>> GetCategoryByIds(List<int> ids);
}
