using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class ShopPromotionRepository : BaseRepository<ShopPromotion>, IShopPromotionRepository
{
    public ShopPromotionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
