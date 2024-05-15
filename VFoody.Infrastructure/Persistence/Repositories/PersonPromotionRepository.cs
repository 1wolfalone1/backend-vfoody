using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class PersonPromotionRepository : BaseRepository<PersonPromotion>, IPersonPromotionRepository
{
    public PersonPromotionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
