using VFoody.Domain.Entities;

namespace VFoody.Application.Common.Repositories;

public interface IShopRepository : IBaseRepository<Shop>
{
    int CountAll();
    List<Shop> GetAllShopIncludeAddress(int pageNum, int pageSize);
    Shop? GetInfoByShopIdAndStatusIn(int shopId, int[] statusList);
    Task<Shop> GetShopByAccountId(int id);
}
