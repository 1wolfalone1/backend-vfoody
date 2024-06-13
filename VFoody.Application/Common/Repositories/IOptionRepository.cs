using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IOptionRepository : IBaseRepository<Option>
{
    Task<bool> CheckExistedOptionByIdsAndQuestionId(List<int> optionIds, int questionId);
    Task<List<Option>> GetByQuestionIds(List<int> questionIds);
}
