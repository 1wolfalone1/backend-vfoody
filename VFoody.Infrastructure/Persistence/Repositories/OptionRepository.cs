using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class OptionRepository : BaseRepository<Option>, IOptionRepository
{
    public OptionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<bool> CheckExistedOptionByIdsAndQuestionId(List<int> optionIds, int questionId)
    {
        // Query the database to get the list of IDs that match the questionId
        var existingOptionIds = await DbSet
            .Where(o => o.QuestionId == questionId && optionIds.Contains(o.Id))
            .Select(o => o.Id)
            .ToListAsync();

        // Check if all optionIds exist in the retrieved list
        return optionIds.All(id => existingOptionIds.Contains(id));
    }

    public async Task<List<Option>> GetByQuestionIds(List<int> questionIds)
    {
        return await DbSet.Where(option => questionIds.Contains(option.QuestionId)).ToListAsync();
    }
}
