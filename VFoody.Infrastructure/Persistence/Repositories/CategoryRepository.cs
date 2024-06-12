using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<bool> CheckExistCategoryByIds(List<int> ids)
    {
        // Count the number of categories that have an ID in the given list
        var matchingCount = await DbSet.CountAsync(category => ids.Contains(category.Id));

        // Return true if the count of matching categories is the same as the count of IDs in the input list
        return matchingCount == ids.Count;
    }

    public async Task<List<Category>> GetCategoryByIds(List<int> ids)
    {
        return await DbSet.Where(category => ids.Contains(category.Id)).ToListAsync();
    }
}