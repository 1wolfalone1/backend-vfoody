using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
{
    public QuestionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
