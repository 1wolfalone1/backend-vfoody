using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class FavouriteShopRepository : BaseRepository<FavouriteShop>, IFavouriteShopRepository
{
    public FavouriteShopRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
