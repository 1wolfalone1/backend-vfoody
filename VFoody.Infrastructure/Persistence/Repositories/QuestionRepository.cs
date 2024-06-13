using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
{
    public QuestionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<bool> CheckExistedQuestionByIdsAndProductId(List<int> questionIds, int productId)
    {
        // Query the database to get the list of IDs that match the productId
        var existingQuestionIds = await DbSet
            .Where(q => q.ProductId == productId && questionIds.Contains(q.Id))
            .Select(q => q.Id)
            .ToListAsync();

        // Check if all questionIds exist in the retrieved list
        return questionIds.All(id => existingQuestionIds.Contains(id));
    }

    public async Task<Question?> GetQuestionIncludeOptionById(int id)
    {
        return await DbSet.Include(question => question.Options).SingleOrDefaultAsync(question => question.Id == id);
    }
}
