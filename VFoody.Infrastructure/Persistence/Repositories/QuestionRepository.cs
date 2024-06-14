using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;
using VFoody.Domain.Enums;

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
            .Where(q => q.ProductId == productId && questionIds.Contains(q.Id) &&
                        q.Status != (int)QuestionStatus.Delete)
            .Select(q => q.Id)
            .ToListAsync();

        // Check if all questionIds exist in the retrieved list
        return questionIds.All(id => existingQuestionIds.Contains(id));
    }

    public async Task<Question?> GetQuestionIncludeOptionById(int id)
    {
        return await DbSet.Include(question => question.Options)
            .SingleOrDefaultAsync(question =>
                question.Id == id && question.Status != (int)QuestionStatus.Delete
            );
    }

    public async Task<List<Question>> GetQuestionByIds(List<int> ids)
    {
        return await DbSet.Where(question => ids.Contains(question.Id)).ToListAsync();
    }

    public async Task<List<Question>> GetQuestionByProductId(int productId)
    {
        return await DbSet.Where(
                question => question.ProductId == productId && question.Status != (int)QuestionStatus.Delete
            )
            .ToListAsync();
    }
}