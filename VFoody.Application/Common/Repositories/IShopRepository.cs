using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IShopRepository : IBaseRepository<Shop>
{
    Shop? GetInfoByShopIdAndStatusIn(int shopId, int[] statusList);
    Task<Shop> GetShopByAccountId(int id);
}
