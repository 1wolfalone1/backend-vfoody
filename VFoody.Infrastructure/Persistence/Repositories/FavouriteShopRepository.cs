using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class FavouriteShopRepository : BaseRepository<FavouriteShop>, IFavouriteShopRepository
{
    public FavouriteShopRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public bool IsFavouriteShop(int shopId, int accountId)
    {
        return DbSet.Any(f => f.ShopId == shopId && f.AccountId == accountId);
    }

    public FavouriteShop? GetByShopIdAndAccountId(int shopId, int accountId)
    {
        return DbSet.FirstOrDefault(f => f.ShopId == shopId && f.AccountId == accountId);
    }

    public async Task<List<FavouriteShop>> GetByAccountId(int accountId)
    {
        return await DbSet.Where(f => f.AccountId == accountId).ToListAsync();
    }
}
