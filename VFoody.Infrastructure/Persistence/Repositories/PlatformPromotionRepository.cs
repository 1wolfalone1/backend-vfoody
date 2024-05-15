using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class PlatformPromotionRepository : BaseRepository<PlatformPromotion>, IPlatformPromotionRepository
{
    public PlatformPromotionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
