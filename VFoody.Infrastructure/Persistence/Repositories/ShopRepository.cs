using Microsoft.EntityFrameworkCore;
using VFoody.Application.Common.Repositories;
using VFoody.Domain.Entities;

namespace VFoody.Infrastructure.Persistence.Repositories;

public class ShopRepository : BaseRepository<Shop>, IShopRepository
{
    public ShopRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public Shop? GetInfoByShopIdAndStatusIn(int shopId, int[] statusList)
    {
        return DbSet.Include(s => s.Building).SingleOrDefault(s => s.Id == shopId && statusList.Contains(s.Status));
    }

    public async Task<Shop> GetShopByAccountId(int id)
    {
        return await DbSet.Where(shop => shop.AccountId == id).SingleAsync();
    }
}