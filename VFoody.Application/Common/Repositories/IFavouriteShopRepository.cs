using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IFavouriteShopRepository : IBaseRepository<FavouriteShop>
{
    bool IsFavouriteShop(int shopId, int accountId);
    FavouriteShop? GetByShopIdAndAccountId(int shopId, int accountId);
    Task<List<FavouriteShop>> GetByAccountId(int accountId);
}
